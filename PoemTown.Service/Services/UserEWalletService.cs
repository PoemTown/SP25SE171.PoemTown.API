using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Transactions;
using PoemTown.Repository.Enums.Wallets;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.UserEWalletRequests;
using PoemTown.Service.BusinessModels.ResponseModels.UserEWalletResponses;
using PoemTown.Service.Events.AnnouncementEvents;
using PoemTown.Service.Events.TransactionEvents;
using PoemTown.Service.Interfaces;
using PoemTown.Service.Scheduler.PaymentJobs;
using PoemTown.Service.ThirdParties.Interfaces;
using Quartz;

namespace PoemTown.Service.Services;

public class UserEWalletService : IUserEWalletService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IZaloPayService _zaloPayService;
    private readonly PaymentMethodFactory _paymentMethodFactory;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ISchedulerFactory _schedulerFactory;
    public UserEWalletService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IZaloPayService zaloPayService,
        PaymentMethodFactory paymentMethodFactory,
        IPublishEndpoint publishEndpoint,
        ISchedulerFactory schedulerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _zaloPayService = zaloPayService;
        _paymentMethodFactory = paymentMethodFactory;
        _publishEndpoint = publishEndpoint;
        _schedulerFactory = schedulerFactory;
    }

    
    public async Task<DepositUserEWalletResponse> DepositUserEWalletAsync(Guid userId, DepositUserEWalletRequest request)
    {
        var paymentMethod = await _paymentMethodFactory.GetPaymentMethod(request.PaymentGatewayId);
        
        // Get user e-wallet, if not exist, create new
        var userEWallet = await _unitOfWork.GetRepository<UserEWallet>().FindAsync(p => p.UserId == userId);
        if (userEWallet == null)
        {
            userEWallet = new()
            {
                UserId = userId,
                WalletBalance = 0,
                WalletStatus = WalletStatus.Active
            };
            await _unitOfWork.GetRepository<UserEWallet>().InsertAsync(userEWallet);
        }
        
        // Create user e-wallet data
        UserEWalletData userEWalletData = new()
        {
            Amount = request.Amount,
            UserId = userId,
            TransactionType = TransactionType.EWalletDeposit,
            UserEWalletId = userEWallet.Id,
        };
        
        var response = await paymentMethod.DepositUserEWalletPayment(userEWalletData);
        
        // Return deposit data is not successful
        if (response.Code != 1)
        {
            return (new DepositUserEWalletResponse()
            {
                Message = response.Message,
                Code = (int)response.Code!,
                IsSuccess = response.IsSuccess
            });
        }

        // Schedule payment timeout job
        await SchedulePaymentTimeOutJob(response.orderCode);
        
        // Return deposit data is successful
        return (new DepositUserEWalletResponse()
        {
            Message = response.Message,
            PaymentUrl = response.PaymentUrl,
            Token = response.Token,
            Code = (int)response.Code!,
            IsSuccess = response.IsSuccess,
        });
    }
    
    private async Task SchedulePaymentTimeOutJob(string orderCode, int minutes = 15, int seconds = 30)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.Start();

        // Create job data map
        JobDataMap jobDataMap = new JobDataMap();
        jobDataMap.Put("orderCode", orderCode);
        
        // Create job
        IJobDetail job = JobBuilder.Create<PaymentTimeOutJob>()
            .WithIdentity(orderCode, "PaymentTimeOutJob")
            .UsingJobData(jobDataMap)
            .Build();

        // Create trigger
        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity(orderCode, "PaymentTimeOutJob")
            .StartAt(DateTimeHelper.SystemTimeNow.AddMinutes(minutes).AddSeconds(seconds))
            .Build();
        
        await scheduler.ScheduleJob(job, trigger);
    }
    
    public async Task DonateUserEWalletAsync(Guid userId, DonateUserEWalletRequest request)
    {
        UserEWallet? userEWallet = await _unitOfWork.GetRepository<UserEWallet>().FindAsync(p => p.UserId == userId);
        // Check user e-wallet exist
        if (userEWallet == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User e-wallet not found");
        }

        User? receiveUser = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == request.ReceiveUserId);
        // Check receive user exists
        if(receiveUser == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Receive user not found");
        }

        // Check donate to yourself
        if(receiveUser.Id == userId)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Cannot donate to yourself");
        }
        
        UserEWallet? receiveUserEWallet = await _unitOfWork.GetRepository<UserEWallet>()
            .FindAsync(p => p.UserId == request.ReceiveUserId);
        
        // Get receive user e-wallet, if not exist, create new
        if (receiveUserEWallet == null)
        {
             receiveUserEWallet = new()
            {
                UserId = receiveUser.Id,
                WalletBalance = 0,
                WalletStatus = WalletStatus.Active
            };
            await _unitOfWork.GetRepository<UserEWallet>().InsertAsync(receiveUserEWallet);
            await _unitOfWork.SaveChangesAsync();
        }
        
        if(userEWallet.WalletBalance < request.Amount)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Insufficient balance in e-wallet");
        }
        // Update user e-wallet balance
        userEWallet.WalletBalance -= request.Amount;
        
        // Update receive user e-wallet balance
        receiveUserEWallet.WalletBalance += request.Amount;
        
        // Create user e-wallet list to update
        List<UserEWallet> userEWallets =
        [
            userEWallet,
            receiveUserEWallet
        ];
        
        _unitOfWork.GetRepository<UserEWallet>().UpdateRange(userEWallets);
        await _unitOfWork.SaveChangesAsync();
        
        // Create user e-wallet transaction
        CreateDonateTransactionEvent createDonateTransactionEvent = new()
        {
            UserEWalletId = userEWallet.Id,
            ReceiveUserEWalletId = receiveUserEWallet.Id,
            Amount = request.Amount,
        };
        
        await _publishEndpoint.Publish(createDonateTransactionEvent);
        
        // Announcement to receive user
        await _publishEndpoint.Publish(new SendUserAnnouncementEvent()
        {
            UserId = receiveUser.Id,
            Title = "Tiền quyên tặng",
            Content = $"Bạn đã nhận được khoản quyên tặng: {request.Amount}VNĐ từ {userEWallet.User.UserName}",
            IsRead = false
        });
    }
    
    public async Task<GetUserEWalletResponse> GetUserEWalletAsync(Guid userId)
    {
        UserEWallet? userEWallet = await _unitOfWork.GetRepository<UserEWallet>().FindAsync(p => p.UserId == userId);
        
        // Check user e-wallet exist
        if (userEWallet == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User e-wallet not found");
        }
        
        return new GetUserEWalletResponse()
        {
            Id = userEWallet.Id,
            WalletBalance = userEWallet.WalletBalance,
            WalletStatus = userEWallet.WalletStatus
        };
    }
}
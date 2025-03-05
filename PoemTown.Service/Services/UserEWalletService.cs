using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Transactions;
using PoemTown.Repository.Enums.Wallets;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.UserEWalletRequests;
using PoemTown.Service.BusinessModels.ResponseModels.UserEWalletResponses;
using PoemTown.Service.Events.TransactionEvents;
using PoemTown.Service.Interfaces;
using PoemTown.Service.ThirdParties.Interfaces;

namespace PoemTown.Service.Services;

public class UserEWalletService : IUserEWalletService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IZaloPayService _zaloPayService;
    private readonly PaymentMethodFactory _paymentMethodFactory;
    private readonly IPublishEndpoint _publishEndpoint;
    public UserEWalletService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IZaloPayService zaloPayService,
        PaymentMethodFactory paymentMethodFactory,
        IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _zaloPayService = zaloPayService;
        _paymentMethodFactory = paymentMethodFactory;
        _publishEndpoint = publishEndpoint;
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
    }
}
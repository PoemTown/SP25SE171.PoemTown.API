using AutoMapper;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Wallets;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.UserEWalletRequests;
using PoemTown.Service.BusinessModels.ResponseModels.UserEWalletResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.ThirdParties.Interfaces;

namespace PoemTown.Service.Services;

public class UserEWalletService : IUserEWalletService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IZaloPayService _zaloPayService;
    private readonly PaymentMethodFactory _paymentMethodFactory;
    public UserEWalletService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IZaloPayService zaloPayService,
        PaymentMethodFactory paymentMethodFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _zaloPayService = zaloPayService;
        _paymentMethodFactory = paymentMethodFactory;
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
            OrderCode = response.OrderCode
        });
    }
}
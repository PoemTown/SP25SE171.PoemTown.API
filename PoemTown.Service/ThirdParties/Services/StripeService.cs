using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Transactions;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.UserEWalletRequests;
using PoemTown.Service.BusinessModels.ResponseModels.UserEWalletResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.ThirdParties.Interfaces;
using PoemTown.Service.ThirdParties.Settings.Stripe;
using Stripe;
using Stripe.Checkout;

namespace PoemTown.Service.ThirdParties.Services;

public class StripeService : IStripeService, IPaymentMethod
{
    private readonly StripeSettings _stripeSettings;
    private readonly IUnitOfWork _unitOfWork;
    public StripeService(StripeSettings stripeSettings, IUnitOfWork unitOfWork)
    {
        _stripeSettings = stripeSettings;
        _unitOfWork = unitOfWork;
        StripeConfiguration.ApiKey = stripeSettings.ApiKey;
    }
    
    public async Task<string> CreatePaymentLink(decimal amount, string orderCode)
    {
        var options = new SessionCreateOptions()
        {
            PaymentMethodTypes = new List<string> { "card" },
            Mode = "payment",
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "vnd",
                        UnitAmount = (long?)(amount), // Stripe requires the amount in cents
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Nạp tiền vào ví điện tử",
                        }
                    },
                    Quantity = 1,
                }
            },
            Metadata = new Dictionary<string, string>
            {
                { "order_code", orderCode }
            },
            SuccessUrl = _stripeSettings.SuccessUrl,
            CancelUrl = _stripeSettings.CancelUrl,
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);
        return session.Url;
    }
    public async Task<DepositUserEWalletResponse> DepositUserEWalletPayment(UserEWalletData request)
    {
        if (request.Amount <= 0)
        {
            return new DepositUserEWalletResponse
            {
                Message = "Số tiền nạp không hợp lệ"
            };
        }
        var orderCode = OrderCodeGenerator.Generate();
        
        var paymentUrl = await CreatePaymentLink(request.Amount, orderCode);

        // Create a new transaction record
        Transaction transaction = new Transaction()
        {
            UserEWalletId = request.UserEWalletId,
            Amount = request.Amount,
            Description = $"Nạp: {request.Amount}VNĐ vào ví điện tử",
            TransactionCode = orderCode,
            Type = TransactionType.EWalletDeposit,
            Status = TransactionStatus.Pending,
            PaymentGateway = await _unitOfWork.GetRepository<PaymentGateway>().FindAsync(p => p.Name == "Stripe"),
        };

        await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
        
        return new DepositUserEWalletResponse
        {
            PaymentUrl = paymentUrl,
            OrderCode = orderCode,
            IsSuccess = true,
            Code = 1,
            Message = "Tạo liên kết thanh toán thành công"
        };
    }
}
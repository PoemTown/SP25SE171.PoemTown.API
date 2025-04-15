using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.Interfaces;
using PoemTown.Service.ThirdParties.Services;

namespace PoemTown.Service.Services;

public class PaymentMethodFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUnitOfWork _unitOfWork;
    public PaymentMethodFactory(IServiceProvider serviceProvider, IUnitOfWork unitOfWork)
    {
        _serviceProvider = serviceProvider;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IPaymentMethod> GetPaymentMethod(Guid paymentGatewayId)
    {
        // Get payment gateway by id, if not found throw exception
        var paymentGateway = await _unitOfWork.GetRepository<PaymentGateway>().FindAsync(p => p.Id == paymentGatewayId);
        if (paymentGateway == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Payment gateway not found or not support");
        }

        // Get payment method by payment gateway name
        IPaymentMethod paymentMethod = paymentGateway.Name switch
        {
            "Zalopay" => _serviceProvider.GetRequiredService<ZaloPayService>(),
            "Vnpay" => _serviceProvider.GetRequiredService<VnPayService>(),
            "Stripe" => _serviceProvider.GetRequiredService<StripeService>(),
            _ => throw new CoreException(StatusCodes.Status400BadRequest, "Payment method not found")
        };

        return paymentMethod;
    }
}
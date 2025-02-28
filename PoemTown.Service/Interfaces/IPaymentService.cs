using PoemTown.Service.BusinessModels.RequestModels.PaymentRequests;

namespace PoemTown.Service.Interfaces;

public interface IPaymentService
{
    Task HandleCallbackPaymentSuccessAsync(HandleCallbackPaymentRequest request);
    Task HandleCallbackPaymentCancelledAsync(HandleCallbackPaymentRequest request);
}
namespace PoemTown.Service.BusinessModels.RequestModels.PaymentGatewayRequests;

public class CreatePaymentGatewayRequest
{
    public string Name { get; set; }
    public string? ImageIcon { get; set; }
    public bool? IsSuspended { get; set; } = false;
}
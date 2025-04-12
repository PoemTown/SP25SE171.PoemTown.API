namespace PoemTown.Service.BusinessModels.RequestModels.PaymentGatewayRequests;

public class UpdatePaymentGatewayRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? ImageIcon { get; set; }
    public bool? IsSuspended { get; set; } = false;
}
namespace PoemTown.Service.BusinessModels.ResponseModels.PaymentGatewayResponses;

public class GetPaymentGatewayResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? ImageIcon { get; set; }
    public bool? IsSuspended { get; set; }
}
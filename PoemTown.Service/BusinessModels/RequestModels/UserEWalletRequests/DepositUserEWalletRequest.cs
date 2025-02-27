using Newtonsoft.Json;

namespace PoemTown.Service.BusinessModels.RequestModels.UserEWalletRequests;

public class DepositUserEWalletRequest
{
    public decimal Amount { get; set; }
    public Guid PaymentGatewayId { get; set; }
}
namespace PoemTown.Service.BusinessModels.RequestModels.UserEWalletRequests;

public class UserEWalletData
{
    public Guid UserId { get; set; }
    public Guid UserEWalletId { get; set; }
    public decimal Amount { get; set; }
}
namespace PoemTown.Service.BusinessModels.RequestModels.UserEWalletRequests;

public class DonateUserEWalletRequest
{
    public decimal Amount { get; set; }
    public Guid ReceiveUserId { get; set; }
}
namespace PoemTown.Service.BusinessModels.RequestModels.UserEWalletRequests;

public class DonateUserEWalletRequest
{
    public decimal Amount { get; set; }
    public Guid ReceiveUserId { get; set; }
    public string? DonateMessage { get; set; } = "Một món quà nho nhỏ từ PoemTown tới bạn";
}
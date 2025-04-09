namespace PoemTown.Service.Events.TransactionEvents;

public class CreateDonateTransactionEvent
{
    public Guid UserEWalletId { get; set; }
    public Guid ReceiveUserEWalletId { get; set; }
    public decimal Amount { get; set; }
    public string? DonateMessage { get; set; } = "Một món quà nho nhỏ từ PoemTown tới bạn";
}
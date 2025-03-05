namespace PoemTown.Service.Events.TransactionEvents;

public class CreateDonateTransactionEvent
{
    public Guid UserEWalletId { get; set; }
    public Guid ReceiveUserEWalletId { get; set; }
    public decimal Amount { get; set; }
}
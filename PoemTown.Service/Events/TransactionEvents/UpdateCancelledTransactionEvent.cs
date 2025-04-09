namespace PoemTown.Service.Events.TransactionEvents;

public class UpdateCancelledTransactionEvent
{
    public string? TransactionCode { get; set; }
}
using PoemTown.Repository.Enums.Transactions;

namespace PoemTown.Service.Events.TransactionEvents;

public class CreateTransactionEvent
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public Guid OrderId { get; set; }
    public TransactionType Type { get; set; }
    public decimal DiscountAmount { get; set; }
    public string? TransactionCode { get; set; }
}
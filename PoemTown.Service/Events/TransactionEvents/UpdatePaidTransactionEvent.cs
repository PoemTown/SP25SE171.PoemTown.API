namespace PoemTown.Service.Events.TransactionEvents;

public class UpdatePaidTransactionEvent
{
    public string? TransactionCode { get; set; }
    public string? BankCode { get; set; }
    public string? AppId { get; set; }
    public decimal? Amount { get; set; }
    public decimal? DiscountAmount { get; set; }
    public string? Checksum { get; set; }
    public decimal? CommissionAmount { get; set; }
}
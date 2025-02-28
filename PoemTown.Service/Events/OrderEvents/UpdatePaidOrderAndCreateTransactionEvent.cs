namespace PoemTown.Service.Events.OrderEvents;

public class UpdatePaidOrderAndCreateTransactionEvent
{
    public string? OrderCode { get; set; }
    public string? BankCode { get; set; }
    public string? AppId { get; set; }
    public decimal? Amount { get; set; }
    public decimal? DiscountAmount { get; set; }
    public string? Checksum { get; set; }
}
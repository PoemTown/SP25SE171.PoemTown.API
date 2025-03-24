using PoemTown.Repository.Enums.Orders;

namespace PoemTown.Service.Events.OrderEvents;

public class CreateOrderEvent
{
    public OrderType Type { get; set; }
    public decimal Amount { get; set; }
    public string OrderCode { get; set; }
    public string OrderDescription { get; set; }
    public OrderStatus Status { get; set; }
    public Guid? MasterTemplateId { get; set; }
    public Guid? RecordFileId { get; set; }
    public Guid? SaleVersionId { get; set; }
    public DateTimeOffset PaidDate { get; set; }
    public decimal DiscountAmount { get; set; }
    public Guid UserId { get; set; }
}
using PoemTown.Repository.Enums.Orders;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;

public class GetDetailsOfOrderResponse
{  
    public Guid Id { get; set; }
    public string OrderCode { get; set; }
    public OrderType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public string OrderDescription { get; set; }
    public OrderStatus Status { get; set; }
    public DateTimeOffset? PaidDate { get; set; }
    public DateTimeOffset? CancelledDate { get; set; }
    public string? OrderToken { get; set; }
    public IList<GetOrderDetailResponse> OrderDetails { get; set; }
}


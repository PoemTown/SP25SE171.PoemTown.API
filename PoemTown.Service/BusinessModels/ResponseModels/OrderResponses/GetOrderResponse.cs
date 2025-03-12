using PoemTown.Repository.Enums.Orders;

namespace PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;

public class GetOrderResponse
{
    public Guid Id { get; set; }
    public string OrderCode { get; set; }
    public OrderType Type { get; set; }
    public decimal Amount { get; set; }
    public string OrderDescription { get; set; }
    public OrderStatus Status { get; set; }
}
using PoemTown.Repository.Enums.Orders;

namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;

public class GetOrderStatusStatisticResponse
{
    public int TotalDataSamples { get; set; }
    public IList<GetOrderTypeSampleResponse>? Samples { get; set; }
}

public class GetOrderTypeSampleResponse
{
    public OrderStatus? Status { get; set; }
    public int TotalOrders { get; set; }
}
using System.Collections;
using PoemTown.Repository.Enums.Orders;

namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;

public class GetOrderTypeStatisticResponse
{
    public int TotalDataSamples { get; set; }
    public decimal? TotalAmounts { get; set; }
    public IList<GetOrderTypeSampleAndAmountResponse>? Samples { get; set; }
}

public class GetOrderTypeSampleAndAmountResponse
{
    public OrderType? OrderType { get; set; }
    public decimal? TotalAmounts { get; set; }
    public int TotalOrders { get; set; }
}

namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;

public class GetTransactionStatisticResponse
{
    public GetStatisticResponse<int, decimal> Samples { get; set; }
}

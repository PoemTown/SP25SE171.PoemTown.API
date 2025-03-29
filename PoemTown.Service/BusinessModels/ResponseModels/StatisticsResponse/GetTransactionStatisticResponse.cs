namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;

public class GetTransactionStatisticResponse
{
    public GetStatisticResponse<int> Samples { get; set; }
    public GetStatisticResponse<decimal> Amounts { get; set; }
}

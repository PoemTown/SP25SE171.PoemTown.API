namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;

public class GetStatisticResponse<TValue>
{
    public TValue TotalDataSamples { get; set; }
    public IList<GetSampleStatisticResponse<TValue>>? Samples { get; set; }
}
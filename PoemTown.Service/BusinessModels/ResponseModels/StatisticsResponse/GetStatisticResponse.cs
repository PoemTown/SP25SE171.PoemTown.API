namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;

public class GetStatisticResponse<TSample, TAmount>
{
    public TSample? TotalDataSamples { get; set; }
    public TAmount? TotalDataAmount { get; set; }
    public IList<GetSampleStatisticResponse<TSample, TAmount>>? Samples { get; set; }
}
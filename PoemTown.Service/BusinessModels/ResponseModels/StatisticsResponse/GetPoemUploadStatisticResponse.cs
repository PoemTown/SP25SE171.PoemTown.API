namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;

public class GetPoemUploadStatisticResponse
{
    public int TotalDataSamples { get; set; }
    public IList<GetSampleStatisticResponse>? Samples { get; set; }
}
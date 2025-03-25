namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticResponse;

public class GetPoemUploadStatisticResponse
{
    public int TotalDataSamples { get; set; }
    public IList<GetSampleStatisticResponse>? Samples { get; set; }
}
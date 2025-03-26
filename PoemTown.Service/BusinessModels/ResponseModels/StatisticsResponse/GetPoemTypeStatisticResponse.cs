using PoemTown.Repository.Enums.Poems;

namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;

public class GetPoemTypeStatisticResponse
{
    public int TotalDataSamples { get; set; }
    public IList<GetPoemTypeSampleResponse>? Samples { get; set; }
}

public class GetPoemTypeSampleResponse
{
    public PoemType? Type { get; set; }
    public int TotalPoems { get; set; }
}
namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;

public class GetTotalStatisticResponse
{
    public int TotalPostedPoems { get; set; }
    public int TotalRecordFiles { get; set; }
    public int TotalCollections { get; set; }
    public int TotalUsers { get; set; }
}
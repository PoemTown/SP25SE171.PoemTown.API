namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;

public class GetOnlineUserStatisticResponse : GetStatisticResponse<int>
{
    /*public int TotalDataSamples { get; set; }
    public IList<GetSampleOnlineUserStatisticResponse>? Samples { get; set; }*/
}

/*public class GetSampleOnlineUserStatisticResponse
{
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public string Period => $"{Year ?? 0}{(Month.HasValue ? $"-{Month.Value:D2}" : "")}{(Day.HasValue ? $"-{Day.Value:D2}" : "")}";
    public int TotalOnlineUsers { get; set; }
}*/
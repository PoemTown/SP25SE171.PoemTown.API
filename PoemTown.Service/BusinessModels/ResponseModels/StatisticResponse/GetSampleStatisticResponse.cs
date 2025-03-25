namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticResponse;

public class GetSampleStatisticResponse
{
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public string Period => $"{Year ?? 0}{(Month.HasValue ? $"-{Month.Value:D2}" : "")}{(Day.HasValue ? $"-{Day.Value:D2}" : "")}";
    public int TotalSamples { get; set; }
}
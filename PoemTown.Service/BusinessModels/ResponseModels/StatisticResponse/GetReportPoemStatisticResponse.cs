using PoemTown.Repository.Enums.Reports;

namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticResponse;

public class GetReportPoemStatisticResponse
{
    public int TotalDataSamples { get; set; }
    public IList<GetReportPoemSampleResponse>? Samples { get; set; }
}

public class GetReportPoemSampleResponse
{
    public ReportStatus Type { get; set; }
    public int TotalPoems { get; set; }
}
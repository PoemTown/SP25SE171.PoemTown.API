using PoemTown.Repository.Enums.Reports;

namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticResponse;

public class GetReportUserStatisticResponse
{
    public int TotalDataSamples { get; set; }
    public IList<GetReportUserSampleResponse>? Samples { get; set; }
}

public class GetReportUserSampleResponse
{
    public ReportStatus Type { get; set; }
    public int TotalUsers { get; set; }
}
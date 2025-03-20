using PoemTown.Repository.Enums.Reports;

namespace PoemTown.Service.BusinessModels.ResponseModels.ReportResponses;

public class GetMyReportResponse
{
    public Guid Id { get; set; }
    public string ReportReason { get; set; }
    public string ResolveResponse { get; set; }
    public ReportStatus Status { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public GetPoemInReportResponse Poem { get; set; }
}
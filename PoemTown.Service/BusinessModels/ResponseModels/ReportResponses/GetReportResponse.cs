using PoemTown.Repository.Enums.Reports;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.ReportResponses;

public class GetReportResponse
{
    public Guid Id { get; set; }
    public string ReportReason { get; set; }
    public ReportStatus Status { get; set; }
    public ReportType Type { get; set; }
    public bool? IsSystem { get; set; }
    public double? PlagiarismScore { get; set; }
    public string? ResolveResponse { get; set; }
    public GetReportUserInReportResponse ReportReportUser { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public GetPoemInReportResponse Poem { get; set; }
    public GetPoemInReportResponse? PlagiarismFromPoem { get; set; }
}
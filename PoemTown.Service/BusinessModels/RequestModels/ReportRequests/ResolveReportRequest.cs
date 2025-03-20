using PoemTown.Repository.Enums.Reports;

namespace PoemTown.Service.BusinessModels.RequestModels.ReportRequests;

public class ResolveReportRequest
{
    public Guid Id { get; set; }
    public string ResolveResponse { get; set; }
    public ReportStatus Status { get; set; }
}
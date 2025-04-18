using PoemTown.Repository.Enums.Reports;

namespace PoemTown.Service.BusinessModels.RequestModels.ReportRequests;

public class UpdateReportMessageRequest
{
    public Guid Id { get; set; }
    public string Description { get; set; } = "";
    public ReportType Type { get; set; }
}
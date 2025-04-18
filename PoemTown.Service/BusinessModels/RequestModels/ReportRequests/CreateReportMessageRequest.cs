using PoemTown.Repository.Enums.Reports;

namespace PoemTown.Service.BusinessModels.RequestModels.ReportRequests;

public class CreateReportMessageRequest
{
    public string Description { get; set; } = "";
    public ReportType Type { get; set; }
}
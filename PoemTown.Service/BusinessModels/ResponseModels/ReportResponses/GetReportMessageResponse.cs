using PoemTown.Repository.Enums.Reports;

namespace PoemTown.Service.BusinessModels.ResponseModels.ReportResponses;

public class GetReportMessageResponse
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public ReportType Type { get; set; }
}
namespace PoemTown.Service.BusinessModels.RequestModels.ReportRequests;

public class CreateReportPoemRequest
{
    public Guid PoemId { get; set; }
    public string? ReportReason { get; set; }
    public Guid? ReportMessageId { get; set; }
}
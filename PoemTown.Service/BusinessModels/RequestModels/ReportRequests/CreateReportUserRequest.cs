namespace PoemTown.Service.BusinessModels.RequestModels.ReportRequests;

public class CreateReportUserRequest
{
    public Guid UserId { get; set; }
    public string? ReportReason { get; set; }
    public Guid? ReportMessageId { get; set; }
}
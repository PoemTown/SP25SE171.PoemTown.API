namespace PoemTown.Service.BusinessModels.RequestModels.ReportRequests;

public class CreateReportUserRequest
{
    public Guid UserId { get; set; }
    public required string ReportReason { get; set; }
}
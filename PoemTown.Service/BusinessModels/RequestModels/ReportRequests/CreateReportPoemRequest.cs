namespace PoemTown.Service.BusinessModels.RequestModels.ReportRequests;

public class CreateReportPoemRequest
{
    public Guid PoemId { get; set; }
    public required string ReportReason { get; set; }
    
}
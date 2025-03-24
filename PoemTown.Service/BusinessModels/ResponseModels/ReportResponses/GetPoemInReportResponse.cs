namespace PoemTown.Service.BusinessModels.ResponseModels.ReportResponses;

public class GetPoemInReportResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    
}
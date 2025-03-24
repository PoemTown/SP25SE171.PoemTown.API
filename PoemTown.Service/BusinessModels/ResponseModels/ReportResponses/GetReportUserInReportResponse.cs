namespace PoemTown.Service.BusinessModels.ResponseModels.ReportResponses;

public class GetReportUserInReportResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Avatar { get; set; }
}
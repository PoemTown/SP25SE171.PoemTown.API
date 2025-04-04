using PoemTown.Repository.Enums.Poems;

namespace PoemTown.Service.BusinessModels.ResponseModels.ReportResponses;

public class GetPoemInReportResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public PoemType? Type { get; set; } = default!;
    public string? PoemImage { get; set; } = null;
    public string? Description { get; set; }
    public double? Score { get; set; }
}
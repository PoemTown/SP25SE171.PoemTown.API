using PoemTown.Repository.Enums.Poems;
using PoemTown.Service.BusinessModels.ResponseModels.PoemTypeResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.ReportResponses;

public class GetPoemInReportResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public GetPoemTypeResponse? Type { get; set; } = default!;
    public string? PoemImage { get; set; } = null;
    public string? Description { get; set; }
    public double? Score { get; set; }
}
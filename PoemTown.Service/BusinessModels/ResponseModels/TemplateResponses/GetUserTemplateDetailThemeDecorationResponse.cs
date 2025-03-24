using PoemTown.Repository.Enums.TemplateDetails;

namespace PoemTown.Service.BusinessModels.ResponseModels.TemplateResponses;

public class GetUserTemplateDetailThemeDecorationResponse
{
    public Guid Id { get; set; }
    public string? ColorCode { get; set; }
    public TemplateDetailType? Type { get; set; }
    public string Name { get; set; }
    public bool? IsInUse { get; set; }
    public string? Image { get; set; }
    public GetUserTemplateResponse UserTemplate { get; set; }
}
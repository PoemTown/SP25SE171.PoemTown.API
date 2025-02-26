using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.ResponseModels.TemplateResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.ThemeResponses;

public class GetThemeResponseV2
{
    public Guid Id { get; set; }
    public bool IsInUse { get; set; }
    public bool IsDefault { get; set; }
    public string Name { get; set; }
    public IList<GetUserTemplateDetailThemeDecorationResponse> UserTemplateDetails { get; set; }
}
using Microsoft.AspNetCore.Mvc;

namespace PoemTown.Service.BusinessModels.RequestModels.TemplateRequests;

public class RemoveUserTemplateDetailInUserThemeRequest
{
    [FromQuery(Name = "themeId")]
    public Guid ThemeId { get; set; }
    [FromQuery(Name = "templateDetailIds")]
    public IList<Guid> TemplateDetailIds { get; set; }
}
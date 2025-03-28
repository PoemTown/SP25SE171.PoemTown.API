namespace PoemTown.Service.BusinessModels.RequestModels.ThemeRequests;

public class ApplyUserTemplateAsThemeRequest
{
    public Guid UserTemplateId { get; set; }
    public Guid ThemeId { get; set; }
}
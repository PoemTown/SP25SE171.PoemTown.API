namespace PoemTown.Service.BusinessModels.RequestModels.TemplateRequests;

public class UpdateThemeUserTemplateDetailRequest
{
    public Guid PreviousUserTemplateDetailId { get; set; }
    public Guid NewUserTemplateDetailId { get; set; }
    public string? NewUserTemplateDetailColorCode { get; set; }
}
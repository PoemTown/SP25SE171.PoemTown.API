using PoemTown.Repository.Enums.Templates;

namespace PoemTown.Service.BusinessModels.RequestModels.TemplateRequests;

public class UpdateMasterTemplateRequest
{
    public Guid Id { get; set; }
    public string? TemplateName { get; set; }
    public decimal? Price { get; set; }
    public string? TagName { get; set; }
    public string? CoverImage { get; set; }
    public TemplateStatus? Status { get; set; }
}
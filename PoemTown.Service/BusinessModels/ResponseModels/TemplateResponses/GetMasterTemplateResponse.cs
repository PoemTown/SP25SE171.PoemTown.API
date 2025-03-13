using PoemTown.Repository.Enums.Templates;

namespace PoemTown.Service.BusinessModels.ResponseModels.TemplateResponses;

public class GetMasterTemplateResponse
{
    public Guid Id { get; set; }
    public string? TemplateName { get; set; }
    public TemplateStatus? Status { get; set; }
    public decimal Price { get; set; }
    public string? TagName { get; set; } = null;
    public bool? IsBought { get; set; }
    public TemplateType? Type { get; set; } = null;
}
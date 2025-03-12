using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.Templates;

namespace PoemTown.Service.BusinessModels.RequestModels.TemplateRequests;

public class CreateMasterTemplateRequest
{
    public string TemplateName { get; set; }
    public decimal? Price { get; set; } = 0;
    public string? TagName { get; set; } = null;
    public string? CoverImage { get; set; }
    public TemplateStatus? Status { get; set; } = TemplateStatus.Inactive;
    public IList<CreateMasterTemplateDetailRequest>? MasterTemplateDetails { get; set; }
}
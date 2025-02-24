using PoemTown.Repository.Enums.Templates;

namespace PoemTown.Service.BusinessModels.ResponseModels.TemplateResponses;

public class GetUserTemplateResponse
{
    public Guid Id { get; set; }
    public string TemplateName { get; set; }
    public string TagName { get; set; }
    public TemplateType Type { get; set; }
}
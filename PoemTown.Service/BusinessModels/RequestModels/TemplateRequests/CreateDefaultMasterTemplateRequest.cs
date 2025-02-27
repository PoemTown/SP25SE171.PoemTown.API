namespace PoemTown.Service.BusinessModels.RequestModels.TemplateRequests;

public class CreateDefaultMasterTemplateRequest
{
    public string TemplateName { get; set; }
    public IList<CreateMasterTemplateDetailRequest>? MasterTemplateDetails { get; set; }
}
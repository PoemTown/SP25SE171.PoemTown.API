namespace PoemTown.Service.BusinessModels.RequestModels.TemplateRequests;

public class AddMasterTemplateDetailIntoMasterTemplateRequest
{
    public Guid MasterTemplateId { get; set; }
    public required List<CreateMasterTemplateDetailRequest> MasterTemplateDetails { get; set; }
}
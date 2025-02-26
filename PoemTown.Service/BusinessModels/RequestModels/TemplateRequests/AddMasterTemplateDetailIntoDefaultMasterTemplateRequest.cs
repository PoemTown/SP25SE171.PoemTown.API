namespace PoemTown.Service.BusinessModels.RequestModels.TemplateRequests;

public class AddMasterTemplateDetailIntoDefaultMasterTemplateRequest
{
    public required List<CreateMasterTemplateDetailRequest> MasterTemplateDetails { get; set; }
}
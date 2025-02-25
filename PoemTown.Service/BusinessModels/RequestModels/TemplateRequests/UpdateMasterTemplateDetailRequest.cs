using PoemTown.Repository.Enums.TemplateDetails;

namespace PoemTown.Service.BusinessModels.RequestModels.TemplateRequests;

public class UpdateMasterTemplateDetailRequest
{
    public Guid Id { get; set; }
    public string? ColorCode { get; set; }
    public string? Image { get; set; }
    public TemplateDetailType Type { get; set; }
}
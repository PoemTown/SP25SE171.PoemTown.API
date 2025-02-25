using PoemTown.Repository.Enums.TemplateDetails;

namespace PoemTown.Service.BusinessModels.ResponseModels.TemplateResponses;

public class GetUserTemplateDetailResponse
{
    public Guid Id { get; set; }
    public string? ColorCode { get; set; }
    public TemplateDetailType? Type { get; set; }
    public string? Image { get; set; }
    //public TemplateDetailDesignType? DesignType { get; set; }
    //public Guid? ParentTemplateDetailId { get; set; }
    public GetUserTemplateResponse UserTemplate { get; set; }
}
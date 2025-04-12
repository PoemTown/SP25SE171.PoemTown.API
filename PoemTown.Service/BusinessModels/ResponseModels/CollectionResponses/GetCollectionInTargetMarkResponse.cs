using PoemTown.Service.BusinessModels.ResponseModels.TargetMarkResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;

public class GetCollectionInTargetMarkResponse
{
    public Guid Id { get; set; }
    public string CollectionName { get; set; }
    public string CollectionDescription { get; set; }
    public string CollectionImage { get; set; }
    public int TotalChapter { get; set; }
    public bool? IsFamousPoet { get; set; } = false;
    public bool? IsCommunity { get; set; }
    public bool? IsMine { get; set; }
    public GetBasicUserInformationResponse User { get; set; }
    public GetTargetMarkResponse TargetMark { get; set; }

}
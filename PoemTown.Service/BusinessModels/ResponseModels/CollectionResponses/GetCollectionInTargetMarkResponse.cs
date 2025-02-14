using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;

public class GetCollectionInTargetMarkResponse
{
    public Guid Id { get; set; }
    public string CollectionName { get; set; }
    public string CollectionDescription { get; set; }
    public string CollectionImage { get; set; }
    public int TotalChapter { get; set; }
    public GetBasicAuthorInformationResponse Author { get; set; }
}
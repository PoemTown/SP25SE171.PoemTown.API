namespace PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;

public class GetCollectionInPoemResponse
{
    public Guid Id { get; set; }
    public string CollectionName { get; set; }
    public bool? IsCommunity { get; set; }
    public bool? IsFamousPoet { get; set; } = false;
}
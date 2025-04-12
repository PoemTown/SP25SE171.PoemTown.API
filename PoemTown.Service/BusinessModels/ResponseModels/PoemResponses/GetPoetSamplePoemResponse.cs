using PoemTown.Repository.Enums.Poems;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.LikeResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoetSampleResponses;
using PoemTown.Service.BusinessModels.ResponseModels.SaleVersionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.TargetMarkResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;

public class GetPoetSamplePoemResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public PoemType Type { get; set; }
    public string Description { get; set; }
    public string? PoemImage { get; set; }
    public bool? IsAbleToUploadRecordFile { get; set; } = false;
    public bool? IsFamousPoet { get; set; } = false;
    public bool IsSellUsageRight { get; set; }
    public PoemStatus Status { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
     
    public GetCollectionInPoemResponse Collection { get; set; }
    public GetPoetSampleResponse PoetSample { get; set; }
}
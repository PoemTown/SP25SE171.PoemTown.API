using PoemTown.Repository.Base;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.LikeResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemHistoryResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemTypeResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoetSampleResponses;
using PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;
using PoemTown.Service.BusinessModels.ResponseModels.SaleVersionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.TargetMarkResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;

public class GetPoemDetailResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public GetPoemTypeResponse? Type { get; set; }
    public string Description { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public int? ChapterNumber { get; set; }
    public string ChapterName { get; set; }
    public string PoemImage { get; set; }
    public decimal Price { get; set; }
    public bool IsSellUsageRight { get; set; }
    public bool? IsFamousPoet { get; set; } = false;
    public bool IsMine { get; set; }
    public bool? IsAbleToUploadRecordFile { get; set; } = false;
    public Guid? SourceCopyRightId { get; set; }
    public PoemStatus Status { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
    public bool? IsFollowed { get; set; } = false;
    public bool? IsAlreadyBought { get; set; } = false;
    public GetCollectionInPoemResponse Collection { get; set; }
    public GetLikeResponse? Like { get; set; }
    public GetTargetMarkResponse? TargetMark { get; set; }
    public GetBasicUserInformationResponse? User { get; set; }
    public GetSaleVersionResponse? SaleVersion { get; set; }
    public PaginationResponse<GetRecordFileResponse>? RecordFiles { get; set; }
    public GetPoetSampleResponse? PoetSample { get; set; }
}
using PoemTown.Repository.Enums.Poems;
using PoemTown.Service.BusinessModels.ResponseModels.LikeResponses;
using PoemTown.Service.BusinessModels.ResponseModels.TargetMarkResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;

public class GetPoemInCollectionResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public PoemType Type { get; set; }
    public string Description { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public int? ChapterNumber { get; set; }
    public string ChapterName { get; set; }
    //public string PoemImage { get; set; }
    public decimal Price { get; set; }
    public bool IsPublic { get; set; }
    public Guid? SourceCopyRightId { get; set; }
    public PoemStatus Status { get; set; }
    public GetBasicUserInformationResponse User { get; set; }
    public GetLikeResponse Like { get; set; }
    public GetTargetMarkResponse TargetMark { get; set; }
}
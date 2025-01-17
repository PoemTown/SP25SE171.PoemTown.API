using PoemTown.Repository.Enums;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;

public class GetPoemResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public PoemType Type { get; set; }
    public string Description { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public int ViewCount { get; set; }
    public int? ChapterNumber { get; set; }
    public string ChapterName { get; set; }
    public PoemStatus Status { get; set; }
    public GetSourceCopyRightResponse SourceCopyRight { get; set; }
}
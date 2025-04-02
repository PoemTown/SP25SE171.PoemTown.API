using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.CommentResponses;

public class GetCommentResponse
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public Guid AuthorCommentId { get; set; }
    public GetBasicUserInformationResponse Author { get; set; }
    public Guid PoemId { get; set; }
    public Guid? ParentCommentId { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
}
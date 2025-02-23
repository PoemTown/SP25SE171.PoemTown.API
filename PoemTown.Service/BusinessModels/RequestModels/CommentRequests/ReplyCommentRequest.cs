namespace PoemTown.Service.BusinessModels.RequestModels.CommentRequests;

public class ReplyCommentRequest
{
    public Guid ParrentCommentId { get; set; }
    public string Content { get; set; } = default!;
}
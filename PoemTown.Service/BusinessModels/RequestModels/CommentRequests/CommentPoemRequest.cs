using Microsoft.AspNetCore.Mvc;

namespace PoemTown.Service.BusinessModels.RequestModels.CommentRequests;

public class CommentPoemRequest
{
    public Guid PoemId { get; set; }
    public string Content { get; set; } = default!;
}
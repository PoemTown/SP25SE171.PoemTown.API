namespace PoemTown.Service.BusinessModels.RequestModels.AnnouncementRequests;

public class CreateSystemAnnouncementRequest
{
    public string? Title { get; set; }
    public string Content { get; set; } = default!;
}
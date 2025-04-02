namespace PoemTown.Service.BusinessModels.RequestModels.AnnouncementRequests;

public class CreateNewAnnouncementRequest
{
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public Guid? UserId { get; set; }
    public bool? IsRead { get; set; } = false;
}
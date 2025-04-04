namespace PoemTown.Service.Events.AnnouncementEvents;

public class UpdateAndSendUserAnnouncementEvent
{
    public Guid UserId { get; set; }
    public Guid AnnouncementId { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public bool IsRead { get; set; } = false;
}
namespace PoemTown.Service.Events.AnnouncementEvents;

public class SendUserAnnouncementEvent
{
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public Guid? UserId { get; set; }
    public bool? IsRead { get; set; } = false;
}
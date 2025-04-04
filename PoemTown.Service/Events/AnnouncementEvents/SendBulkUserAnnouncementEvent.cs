namespace PoemTown.Service.Events.AnnouncementEvents;

public class SendBulkUserAnnouncementEvent
{
    public List<Guid>? UserIds { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public bool IsRead { get; set; } = false;
}
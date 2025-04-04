using PoemTown.Repository.Enums.Announcements;

namespace PoemTown.Service.Events.AnnouncementEvents;

public class UpdateAndSendUserAnnouncementEvent
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = default!;
    public AnnouncementType Type { get; set; }
    public string Content { get; set; } = default!;
    public bool? IsRead { get; set; } = false;
}
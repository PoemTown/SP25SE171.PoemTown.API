using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class SystemAnnouncement : BaseEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    
    public virtual ICollection<Announcement>? Announcements { get; set; }
}
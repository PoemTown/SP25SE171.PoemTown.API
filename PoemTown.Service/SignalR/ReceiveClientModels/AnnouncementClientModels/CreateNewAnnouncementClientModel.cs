namespace PoemTown.Service.SignalR.ReceiveClientModels.AnnouncementClientModels;

public class CreateNewAnnouncementClientModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public bool? IsRead { get; set; } = false;
    public DateTimeOffset CreatedTime { get; set; }
}
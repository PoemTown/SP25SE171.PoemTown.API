namespace PoemTown.Service.BusinessModels.ResponseModels.AnnouncementResponses;

public class GetAnnouncementResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public Guid? UserId { get; set; }
    public bool IsRead { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
}
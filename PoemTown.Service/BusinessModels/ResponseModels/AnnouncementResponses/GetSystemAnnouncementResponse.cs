namespace PoemTown.Service.BusinessModels.ResponseModels.AnnouncementResponses;

public class GetSystemAnnouncementResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public DateTimeOffset CreatedTime { get; set; }
}
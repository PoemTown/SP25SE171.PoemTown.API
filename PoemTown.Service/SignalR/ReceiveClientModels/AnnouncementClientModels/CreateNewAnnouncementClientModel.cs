using PoemTown.Repository.Enums.Announcements;

namespace PoemTown.Service.SignalR.ReceiveClientModels.AnnouncementClientModels;

public class CreateNewAnnouncementClientModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public bool? IsRead { get; set; } = false;
    public DateTimeOffset CreatedTime { get; set; }
    public AnnouncementType? Type { get; set; }
    public Guid? ReportId { get; set; }
    public Guid? CollectionId { get; set; }
    public Guid? PoemId { get; set; }
    public Guid? CommentId { get; set; }
    public Guid? LikeId { get; set; }
    public Guid? TransactionId { get; set; }
    public Guid? AchievementId { get; set; }
    public Guid? PoemLeaderboardId { get; set; }
    public Guid? UserLeaderboardId { get; set; }
    public Guid? FollowerId { get; set; }
    public Guid? FollowerUserId { get; set; }
    public Guid? RecordFileId { get; set; }
}
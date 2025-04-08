using PoemTown.Repository.Enums.Announcements;

namespace PoemTown.Service.BusinessModels.ResponseModels.AnnouncementResponses;

public class GetAnnouncementResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public AnnouncementType Type { get; set; }
    public Guid? UserId { get; set; }
    public bool IsRead { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public Guid? ReportId { get; set; }
    public Guid? CollectionId { get; set; }
    public Guid? PoemId { get; set; }
    public Guid? CommentId { get; set; }
    public Guid? LikeId { get; set; }
    public Guid? TransactionId { get; set; }
    public Guid? AchievementId { get; set; }
    public Guid? PoemLeaderboardId { get; set; }
    public Guid? UserLeaderboardId { get; set; }
    public Guid? RecordFileId { get; set; }
    public Guid? FollowerId { get; set; }
    public string? FollowerUserName { get; set; }
}
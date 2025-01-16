using Microsoft.AspNetCore.Identity;
using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;

namespace PoemTown.Repository.Entities;

public class User : IdentityUser<Guid>, IBaseEntity
{
    public string? FullName { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Salt { get; set; }
    public string? GoogleId { get; set; }
    
    public string? EmailOtp { get; set; }
    public string? EmailOtpExpiration { get; set; }
    
    public string? PhoneOtp { get; set; }
    public string? PhoneOtpExpiration { get; set; }

    public virtual UserEWallet? EWallet { get; set; }
    public virtual LeaderBoardDetail? LeaderBoardDetail {  get; set; }
    public virtual ICollection<UserLeaderBoard>? UserLeaderBoards { get; set; }

    public virtual ICollection<Collection>? Collections { get; set; }
    public virtual ICollection<UserPoem>? UserPoems { get; set; }
    public virtual ICollection<TargetMark>? MarkByUsers { get; set; }
    public virtual ICollection<TargetMark>? MarkedUsers { get; set; }
    public virtual ICollection<Follower>? FollowUser { get; set; }
    public virtual ICollection<Follower>? FollowedUser { get; set; }
    public virtual ICollection<Report>? ReportUsers { get; set; }
    public virtual ICollection<Report>? ReportedUsers { get; set; }
    public virtual ICollection<RecordFile>? RecordFiles { get; set; }

    public virtual ICollection<Comment>? AuthorComments { get; set; }
 /*   public virtual ICollection<Comment> ParentComments { get; set; }*/
    public virtual ICollection<Message>? FromUser { get; set; }
    public virtual ICollection<Message>? ToUser { get; set; }
    public virtual ICollection<UserCopyRight>? UserCopyRights { get; set; }
    public virtual ICollection<Order>? Orders { get; set; }
    public virtual ICollection<UserTemplate>? UserTemplates { get; set; }
    public virtual ICollection<Transaction>? Transactions { get; set; }



    public virtual ICollection<Announcement>? Announcements { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastUpdatedBy { get; set; }
    public string? DeletedBy { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset LastUpdatedTime { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }



    public User()
    {
        CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
    }
}
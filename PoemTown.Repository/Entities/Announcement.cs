using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Repository.Base;
using PoemTown.Repository.Enums;
using PoemTown.Repository.Enums.Announcements;

namespace PoemTown.Repository.Entities
{
    public class Announcement : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public AnnouncementType? Type { get; set; } = default!;
        public bool? IsRead { get; set; } = false;
        
        // User
        public Guid? UserId { get; set; }
        public virtual User? User { get; set; }
        
        // Report
        public Guid? ReportId { get; set; }
        public virtual Report? Report { get; set; }
        
        // Collection
        public Guid? CollectionId { get; set; }
        public virtual Collection? Collection { get; set; }
        
        // Poem
        public Guid? PoemId { get; set; }
        public virtual Poem? Poem { get; set; }
        
        // Comment
        public Guid? CommentId { get; set; }
        public virtual Comment? Comment { get; set; }
        
        // Like
        public Guid? LikeId { get; set; }
        public virtual Like? Like { get; set; }
        
        // Transaction
        public Guid? TransactionId { get; set; }
        public virtual Transaction? Transaction { get; set; }
        
        // Achievement
        public Guid? AchievementId { get; set; }
        public virtual Achievement? Achievement { get; set; }
        
        // PoemLeaderBoard
        public Guid? PoemLeaderboardId { get; set; }
        public virtual PoemLeaderBoard? PoemLeaderboard { get; set; }
        
        // UserLeaderBoard
        public Guid? UserLeaderboardId { get; set; }
        public virtual UserLeaderBoard? UserLeaderboard { get; set; }
        
        // RecordFile
        public Guid? RecordFileId { get; set; }
        public virtual RecordFile? RecordFile { get; set; }
        
        // Follower
        public Guid? FollowerId { get; set; }
        public virtual Follower? Follower { get; set; }
    }
}

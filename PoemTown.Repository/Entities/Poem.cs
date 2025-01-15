using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Repository.Entities
{
    public class Poem : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? Title {  get; set; }
        public string? Content { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public int? LikeCount { get; set; }
        public int? CommentCount { get; set; }
        public int? ViewCount { get; set; }
        public int? ChapterNumber { get; set; }
        public string? ChapterName { get; set; }
        public string? PoemStatus { get; set; }
        public string? PoemImage {  get; set; }
        public int? SourceCopyRight { get; set; }
        public Guid UserId { get; set; }
        public Guid CollectionId { get; set; }
        public virtual ICollection<UserPoems> UserPoems { get; set; }
        public virtual Collection Collection { get; set; }
/*        public virtual CopyRight CopyRight { get; set; }*/
        public virtual ICollection<PoemHistory> PoemHistories { get; set; }
        public virtual ICollection<RecordFile> RecordFiles { get; set; }
        public virtual ICollection<TargetMark> TargetMarks { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<UserCopyRight>? UserCopyRights { get; set; }

        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public Poem()
        {
            CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
        }
    }
}

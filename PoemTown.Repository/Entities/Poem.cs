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

namespace PoemTown.Repository.Entities
{
    public class Poem : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string? Title { get; set; } = "";
        public string? Content { get; set; } = "";
        public PoemType? Type { get; set; } = default!;
        public string? Description { get; set; } = "";
        public int? LikeCount { get; set; } = 0;
        public int? CommentCount { get; set; } = 0;
        public int? ViewCount { get; set; } = 0;
        public int? ChapterNumber { get; set; }
        public string? ChapterName { get; set; } = "";
        public PoemStatus? PoemStatus { get; set; } = default;
        public string? PoemImage { get; set; } = null;
        public Guid? SourceCopyRight { get; set; } = null;
        public bool? IsDraft { get; set; } = true;
        public Guid UserId { get; set; }
        public Guid? CollectionId { get; set; }
        public virtual User User { get; set; }
        public virtual Collection? Collection { get; set; } = null;
        public virtual CopyRight? CopyRight { get; set; } = null;
        public virtual ICollection<PoemHistory>? PoemHistories { get; set; } = null;
        public virtual ICollection<RecordFile>? RecordFiles { get; set; } = null;
        public virtual ICollection<TargetMark>? TargetMarks { get; set; } = null;
        public virtual ICollection<Report>? Reports { get; set; } = null;
        public virtual ICollection<Comment>? Comments { get; set; } = null;
    }
}

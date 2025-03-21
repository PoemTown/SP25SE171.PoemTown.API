using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Repository.Base;
using PoemTown.Repository.Enums;
using PoemTown.Repository.Enums.Poems;

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
        public int? ChapterNumber { get; set; }
        public string? ChapterName { get; set; } = "";
        public PoemStatus? Status { get; set; } = PoemStatus.Draft;
        public string? PoemImage { get; set; } = null;
        public decimal Price { get; set; }
        public Guid? SourceCopyRightId { get; set; }
        public Guid? CollectionId { get; set; }
        public Guid? UserId { get; set; }
        public bool? IsSellCopyRight { get; set; } = false;
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        public virtual Collection? Collection { get; set; }
        public virtual OrderDetail? OrderDetail { get; set; }
        public virtual ICollection<PoemLeaderBoard> PoemLeaderBoards { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
        /*        public virtual CopyRight CopyRight { get; set; }*/
        //public virtual ICollection<UserPoemRecordFile>? UserPoemRecordFiles { get; set; }

        public virtual ICollection<PoemHistory>? PoemHistories { get; set; }
        public virtual ICollection<RecordFile>? RecordFiles { get; set; }
        public virtual ICollection<TargetMark>? TargetMarks { get; set; }
        public virtual ICollection<Report>? Reports { get; set; }
        public virtual ICollection<Report>? PlagiarismReports { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<SaleVersion>? SaleVersions { get; set; }
        /*
        public virtual ICollection<UserCopyRight>? UserCopyRights { get; set; }
    */
    }
}

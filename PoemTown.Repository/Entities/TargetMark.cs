using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Repository.Entities
{
    public class TargetMark : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? Type { get; set; }
        public string? TargetContent { get; set; }
        public Guid CollectionId { get; set; }
        public Guid PoemId { get; set; }
        public Guid MarkByUserId { get; set; }
        public Guid MarkedUserId { get; set; }
        [ForeignKey("MarkedUserId")]
        public virtual User MarkedUser { get; set; }
        [ForeignKey("MarkByUserId")]
        public virtual User MarkByUser { get; set; }
        public virtual Poem Poem { get; set; }
        public virtual Collection Collection { get; set; }


        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public TargetMark()
        {
            CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
        }
    }
}

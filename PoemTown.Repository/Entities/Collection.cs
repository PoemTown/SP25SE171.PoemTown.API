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
    public class Collection : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? CollectionName { get; set; }
        public string? CollectionDescription { get; set; }
        public int? TotalChapter { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; } 

        public virtual ICollection<Poem> Poems { get; set; }
        public virtual ICollection<TargetMark> TargetMarks { get; set; }

        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public Collection()
        {
            CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
        }

    }
}

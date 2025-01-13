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
    public class CopyRight : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public decimal? Price { get; set; }
        public int? SourceCopyRight { get; set; }

        public Guid PoemId { get; set; }
        public virtual Poem Poem { get; set; }
        public virtual OrderDetail OrderDetail { get; set; }
        public virtual ICollection<UserCopyRight> UserCopyRights { get; set; }

        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public CopyRight()
        {
            CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
        }
    }
}

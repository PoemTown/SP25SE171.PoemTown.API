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
    public class Report : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? ReportReason { get; set; }
        public string? Status { get; set; }
        public bool? IsSystem { get; set; }

        public Guid ReportUserId { get; set; }
        public Guid ReportedUserId { get; set; }

        public Guid PoemId { get; set; }

        

        [ForeignKey("ReportUserId")]
        public virtual User ReportUser { get; set; }
        [ForeignKey("ReportedUserId")]
        public virtual User ReportedUser { get; set; }

        public virtual Poem Poem { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public Report()
        {
            CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
        }
    }
}

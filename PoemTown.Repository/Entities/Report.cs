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
using PoemTown.Repository.Enums.Reports;

namespace PoemTown.Repository.Entities
{
    public class Report : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string ReportReason { get; set; } = default!;
        public ReportStatus Status { get; set; } = ReportStatus.Pending;
        public bool? IsSystem { get; set; } = false;

        public Guid? ReportUserId { get; set; }
        public Guid? ReportedUserId { get; set; }

        public Guid? PoemId { get; set; }



        [ForeignKey("ReportUserId")] 
        public virtual User? ReportUser { get; set; }

        [ForeignKey("ReportedUserId")]
        public virtual User? ReportedUser { get; set; }

        public virtual Poem? Poem { get; set; }
    }
}

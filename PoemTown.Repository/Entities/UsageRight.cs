using PoemTown.Repository.Base;
using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Repository.Enums;
using PoemTown.Repository.Enums.UsageRights;
using PoemTown.Repository.Enums.UserPoems;

namespace PoemTown.Repository.Entities
{
    public class UsageRight : BaseEntity
    {
        [Key] 
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }
        //public Guid? PoemId { get; set; }
        public Guid? RecordFileId { get; set; }
        public UsageRightStatus Status { get; set; }
        public UserPoemType? Type { get; set; }
        
        public DateTime? CopyRightValidFrom { get; set; }
        
        public DateTime? CopyRightValidTo { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        public Guid? SaleVersionId { get; set; }
        public virtual SaleVersion? SaleVersion { get; set; }

        /*[ForeignKey("PoemId")]
        public virtual Poem? Poem { get; set; }*/
        [ForeignKey("RecordFileId")]
        public virtual RecordFile? RecordFile { get; set; }
    }
}

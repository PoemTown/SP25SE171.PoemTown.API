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
    public class UserCopyRight : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime ExperiedTime { get; set; }  
        public Guid UserId { get; set; }
        public Guid PoemId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("PoemId")]
        public virtual Poem Poem { get; set; }


        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public UserCopyRight()
        {
            CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
        }
    }
}

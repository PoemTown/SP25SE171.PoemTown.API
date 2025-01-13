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
    public class Message : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string MessageText { get; set; }
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }

        [ForeignKey("FromUserId")]
        public virtual User FromUser { get; set; }
        [ForeignKey("ToUserId")]
        public virtual User ToUser { get; set; }


        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public Message()
        {
            CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
        }
    }
}

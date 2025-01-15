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
    public class Followers : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid FollowUserId { get; set; }
        public Guid FollowedUserId { get; set; }
        [ForeignKey("FollowUserId")]
        public virtual User FollowUser { get; set; }
        [ForeignKey("FollowedUserId")]
        public virtual User FollowedUser { get; set; }


        public virtual ICollection<Announcement>? Announcements { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }



        public Followers()
        {
            CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
        }
    }
}

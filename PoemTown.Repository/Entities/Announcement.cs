using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Repository.Base;
using PoemTown.Repository.Enums;

namespace PoemTown.Repository.Entities
{
    public class Announcement : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public AnnouncementType? Type { get; set; } = default!;
        public bool? IsRead { get; set; } = false;
        public Guid? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}

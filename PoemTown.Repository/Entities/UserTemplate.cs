using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities
{
    public class UserTemplate : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TemplateId { get; set; }
        public bool? IsUse { get; set; } = false;
        public virtual User User { get; set; }
        public virtual Template Template { get; set; }
    }
}

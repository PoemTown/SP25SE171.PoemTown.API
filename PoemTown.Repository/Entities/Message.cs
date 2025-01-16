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

namespace PoemTown.Repository.Entities
{
    public class Message : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string MessageText { get; set; } = default!;
        public Guid FromUserId { get; set; } = default;
        public Guid ToUserId { get; set; } = default;

        [ForeignKey("FromUserId")]
        public virtual User FromUser { get; set; } = default!;
        [ForeignKey("ToUserId")]
        public virtual User ToUser { get; set; } = default!;
        
    }
}

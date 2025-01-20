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

namespace PoemTown.Repository.Entities
{
    public class TargetMark : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public TargetMarkType Type { get; set; } = default;
        public string? TargetContent { get; set; } = "";
        public Guid? CollectionId { get; set; }
        public Guid? PoemId { get; set; }
        public Guid? MarkByUserId { get; set; }
        [ForeignKey("MarkByUserId")]
        public virtual User? MarkByUser { get; set; }
        public virtual Poem? Poem { get; set; }
        public virtual Collection? Collection { get; set; }
    }
}

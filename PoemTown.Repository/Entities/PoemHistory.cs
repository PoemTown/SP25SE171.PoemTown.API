using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Repository.Base;
using PoemTown.Repository.Enums.Poems;

namespace PoemTown.Repository.Entities
{
    public class PoemHistory : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public int? Version { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public PoemStatus? Status { get; set; }
        public string? Description { get; set; }
        public int? ChapterNumber { get; set; }
        public string? ChapterName { get; set; }
        public string? PoemImage { get; set; } 
        public decimal Price { get; set; }
        public Guid? SourceCopyRightId { get; set; }
        public bool? IsPublic { get; set; }
        public Guid PoemId { get; set; }
        
        public Guid? PoemTypeId { get; set; }
        public virtual PoemType? Type { get; set; }
        public virtual Poem Poem { get; set; } = default!;
    }
}

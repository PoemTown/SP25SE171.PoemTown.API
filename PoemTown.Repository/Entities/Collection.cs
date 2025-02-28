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
    public class Collection : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string CollectionName { get; set; } = default!;
        public string? CollectionDescription { get; set; } = default!;
        public string? CollectionImage { get; set; } = default!;
        public bool? IsDefault { get; set; } = false;
        public int? TotalChapter { get; set; } = default!;
        public bool IsCommunity { get; set; } = false;
        public Guid UserId { get; set; } = default;
        public virtual User User { get; set; } = default!;

        public virtual ICollection<Poem>? Poems { get; set; } = null;
        public virtual ICollection<TargetMark>? TargetMarks { get; set; } = null;


    }
}

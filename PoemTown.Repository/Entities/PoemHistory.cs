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
    public class PoemHistory : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string? Version { get; set; } = "";
        public Guid PoemId { get; set; } = default;
        public virtual Poem Poem { get; set; } = default!;
    }
}

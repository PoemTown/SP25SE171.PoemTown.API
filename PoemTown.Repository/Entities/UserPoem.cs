using PoemTown.Repository.Base;
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
    public class UserPoem : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public Guid PoemId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("PoemId")]
        public virtual Poem? Poem { get; set; }
    }
}

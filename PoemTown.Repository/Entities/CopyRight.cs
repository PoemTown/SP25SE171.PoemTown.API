/*using PoemTown.Repository.Base.Interfaces;
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
    public class CopyRight : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public decimal? Price { get; set; } = 0;
        public int? SourceCopyRight { get; set; } = null;

        public Guid PoemId { get; set; } = default;
        public virtual Poem Poem { get; set; } = default!;
        public virtual OrderDetail? OrderDetail { get; set; } = null;
        public virtual ICollection<UserCopyRight>? UserCopyRights { get; set; } = null;
    }
}
*/
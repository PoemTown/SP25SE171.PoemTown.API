using PoemTown.Repository.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Repository.Entities
{
    public class PoemLeaderBoard : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? PoemId { get; set; }
        public Guid? LeaderBoardId { get; set; }
        public int Rank { get; set; }
        public virtual Poem? Poem { get; set; }
        public virtual LeaderBoard? LeaderBoard { get; set; }
    }
}

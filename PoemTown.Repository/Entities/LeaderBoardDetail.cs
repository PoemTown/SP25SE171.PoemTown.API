using PoemTown.Repository.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Repository.Entities
{
    public class LeaderBoardDetail : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PoemId { get; set; }
        public Guid? LeaderBoardId { get; set; }

        public User? User { get; set; }
        public Poem? Poem { get; set; }
        public LeaderBoard? LeaderBoard { get; set; }
    }
}

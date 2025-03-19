using PoemTown.Repository.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Repository.Entities
{
    public class UserLeaderBoard : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? LeaderBoardId { get; set; }
        public int Rank { get; set; }
        public virtual User? User { get; set; }
        public virtual LeaderBoard? LeaderBoard {  get; set; }
    }
}

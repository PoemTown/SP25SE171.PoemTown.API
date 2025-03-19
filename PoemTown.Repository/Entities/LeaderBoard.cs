using PoemTown.Repository.Base;
using PoemTown.Repository.Enums.LeaderBoards;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Repository.Entities
{
    public class LeaderBoard : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public LeaderBoardType Type { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set;}
        public LeaderBoardStatus Status { get; set; }
        public virtual ICollection<LeaderBoardDetail> LeaderBoardDetails { get; set; }
        public virtual ICollection<UserLeaderBoard> UserLeaderBoards { get; set; }
    }
}

using PoemTown.Repository.Base;
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
        public string? Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set;}
        public virtual ICollection<LeaderBoardDetail> LeaderBoardDetails { get; set; }
        public virtual ICollection<UserLeaderBoard> UserLeaderBoards { get; set; }

    }
}

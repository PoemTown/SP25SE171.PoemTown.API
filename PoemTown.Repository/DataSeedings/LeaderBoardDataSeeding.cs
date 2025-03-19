using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.LeaderBoards;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Repository.DataSeedings
{
    public class LeaderBoardDataSeeding
    {
        public static IList<LeaderBoard> DefaultLeaderBoards
        {
           get
            {
                var now = DateTimeHelper.SystemTimeNow;
                var startOfMonth = new DateTime(now.Year, now.Month, 1);
                var endOfMonth = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59);
                
                return new List<LeaderBoard>
                {
                    new LeaderBoard()
                    {
                        Id = new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                        Type = LeaderBoardType.Poem,
                        StartDate = startOfMonth,
                        EndDate = endOfMonth,
                        Status = LeaderBoardStatus.InProgress,
                    },
                    new LeaderBoard()
                    {
                        Id = new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                        Type = LeaderBoardType.User,
                        StartDate = startOfMonth,
                        EndDate = endOfMonth,
                        Status = LeaderBoardStatus.InProgress,
                    }
                };
            }
 
        }
    }
}

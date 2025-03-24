using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.LeaderBoards;
using PoemTown.Service.BusinessModels.ResponseModels.LeaderBoardDetailResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserLeaderBoardResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.ResponseModels.LeaderBoardResponses
{
    public class GetLeaderBoardResponse
    {
        public Guid Id { get; set; }
        public LeaderBoardType Type { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public LeaderBoardStatus Status { get; set; }
        public ICollection<GetLeaderBoardDetailResponse>? TopPoems { get; set; }
        public ICollection<GetUserLeaderBoardResponse>? TopUsers { get; set; }

    }
}

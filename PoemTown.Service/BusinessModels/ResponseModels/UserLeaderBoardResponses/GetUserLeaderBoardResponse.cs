using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.ResponseModels.UserLeaderBoardResponses
{
    public class GetUserLeaderBoardResponse
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? LeaderBoardId { get; set; }
        public int Rank { get; set; }
        public GetUserProfileResponse User { get; set; }
    }
}

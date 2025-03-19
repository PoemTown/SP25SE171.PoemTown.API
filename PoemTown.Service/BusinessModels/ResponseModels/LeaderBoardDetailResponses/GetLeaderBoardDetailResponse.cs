using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.ResponseModels.LeaderBoardDetailResponses
{
    public class GetLeaderBoardDetailResponse
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PoemId { get; set; }
        public Guid? LeaderBoardId { get; set; }
        public GetUserProfileResponse User { get; set; }
        public GetPoemResponse Poem { get; set; }
    }
}

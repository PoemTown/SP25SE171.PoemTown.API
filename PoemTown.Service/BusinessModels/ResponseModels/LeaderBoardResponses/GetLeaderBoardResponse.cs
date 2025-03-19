using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.LeaderBoardDetailResponses;
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
        public string? Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<GetLeaderBoardDetailResponse> LeaderBoardDetails { get; set; }

    }
}

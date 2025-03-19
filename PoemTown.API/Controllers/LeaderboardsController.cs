using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.LeaderBoardDetailResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.Services;

namespace PoemTown.API.Controllers
{
    public class LeaderboardsController : BaseController
    {
        private readonly ILeaderBoardService _leaderBoardService;
        
        public LeaderboardsController(ILeaderBoardService leaderBoardService)
        {
            _leaderBoardService = leaderBoardService;
        }

       /* [HttpGet]
        [Route("v1/leaderboard")]
        public async Task<ActionResult<BasePaginationResponse<GetLeaderBoardDetailResponse>>> GetTopPoemsLeaderBoard()
        {
            var paginationResponse = await _leaderBoardService.CalculateTopPoemsAsync();

        } */
    }
}

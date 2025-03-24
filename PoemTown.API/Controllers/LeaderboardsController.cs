using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.LeaderBoardDetailResponses;
using PoemTown.Service.BusinessModels.ResponseModels.LeaderBoardResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.LeaderBoardFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.LeaderBoardSorts;
using PoemTown.Service.Services;

namespace PoemTown.API.Controllers
{
    public class LeaderboardsController : BaseController
    {
        private readonly ILeaderBoardService _leaderBoardService;
        private readonly IMapper _mapper;
        
        public LeaderboardsController(ILeaderBoardService leaderBoardService, IMapper mapper)
        {
            _leaderBoardService = leaderBoardService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lấy danh sách Top bài thơ trên bảng xếp hạng, không yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// CHÚ Ý REQUEST PARAMETER:
        ///
        /// - tất cả lấy từ request query
        /// 
        /// Status: Trạng thái bảng xếp hạng
        ///
        /// - 0: InProgress
        /// - 1: Done
        ///
        /// SortOptions: Sắp xếp bài thơ theo thứ tự
        ///
        /// - 0: Xếp hạng tăng dần (top 1 - top 20)
        /// - 1: Xếp hạng giảm dần (top 20 - top 1)
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/poem-leaderboard")]
        public async Task<ActionResult<BasePaginationResponse<GetLeaderBoardResponse>>> GetTopPoemsLeaderBoard(RequestOptionsBase<GetLeaderBoardFilterOption, GetLeaderBoardSortOption> request)
        {
            var paginationResponse = await _leaderBoardService.GetTopPoemsLeaderBoard(request);
            
            var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetLeaderBoardResponse>>(paginationResponse);
            basePaginationResponse.StatusCode = StatusCodes.Status200OK;
            basePaginationResponse.Message = "Get Top Poems LeaderBoard successfully";

            return Ok(basePaginationResponse);
        }

        /// <summary>
        /// Lấy danh sách Top Người dùng trên bảng xếp hạng, không yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// CHÚ Ý REQUEST PARAMETER:
        ///
        /// - tất cả lấy từ request query
        /// 
        /// Status: Trạng thái bảng xếp hạng
        ///
        /// - 0: InProgress
        /// - 1: Done
        ///
        /// SortOptions: Sắp xếp người dùng theo thứ tự
        ///
        /// - 0: Xếp hạng tăng dần (top 1 - top 20)
        /// - 1: Xếp hạng giảm dần (top 20 - top 1)
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/user-leaderboard")]
        public async Task<ActionResult<BasePaginationResponse<GetLeaderBoardResponse>>> GetTopUsersLeaderBoard(RequestOptionsBase<GetLeaderBoardFilterOption, GetLeaderBoardSortOption> request)
        {
            var paginationResponse = await _leaderBoardService.GetTopUsersLeaderBoard(request);

            var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetLeaderBoardResponse>>(paginationResponse);
            basePaginationResponse.StatusCode = StatusCodes.Status200OK;
            basePaginationResponse.Message = "Get Top Users LeaderBoard successfully";

            return Ok(basePaginationResponse);

        }
    }
}

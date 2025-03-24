using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.ResponseModels.AchievementRespponses;
using PoemTown.Service.BusinessModels.ResponseModels.CommentResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.AchievementFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.AchievementSorts;

namespace PoemTown.API.Controllers
{
    public class AchievementsController : BaseController
    {
        private readonly IAchievementService _achievementService;
        private readonly IMapper _mapper;
        public AchievementsController(IAchievementService achievementService, IMapper mapper)
        {
            _achievementService = achievementService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lấy danh sách thành tựu của người dùng, yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// CHÚ Ý REQUEST PARAMETER:
        ///
        /// - tất cả lấy từ request query
        /// 
        /// FilterOptions: Lọc thành tựu
        /// 
        /// Rank:
        /// - 1
        /// - 2
        /// - 3
        /// - ...
        /// - 20
        /// 
        /// Type:
        ///
        /// - Poem = 1,
        /// - User = 2
        ///
        /// SortOptions: Sắp xếp bài thơ theo thứ tự
        ///
        /// - 1: EarnedDateDescending (Ngày nhận giảm dần)
        /// - 2: EarnedDateAscending (Ngày nhận tăng dần)
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/mine")]
        [Authorize]
        public async Task<ActionResult<BasePaginationResponse<GetAchievementResponse>>> GetMyAchievements(RequestOptionsBase<GetAchievementFilterOption, GetAchievementSortOption> request)
        {
            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
            var paginationResponse = await _achievementService.GetMyAchievements(userId, request);

            var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetAchievementResponse>>(paginationResponse);
            basePaginationResponse.StatusCode = StatusCodes.Status200OK;
            basePaginationResponse.Message = "Get comments successfully";

            return Ok(basePaginationResponse);
        }
    }
}

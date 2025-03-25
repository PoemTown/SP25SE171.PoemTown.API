using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.CollectionRequest;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.StatisticResponse;
using PoemTown.Service.Interfaces;
using System.Security.Claims;
using PoemTown.Service.QueryOptions.FilterOptions.StatisticFilters;

namespace PoemTown.API.Controllers
{
    public class StatisticsController : BaseController
    {
        private readonly IStatisticService _service;
        private readonly IMapper _mapper;
        public StatisticsController(IMapper mapper, IStatisticService service)
        {
            _mapper = mapper;
            _service = service;
        }

        /// <summary>
        /// Lấy thống kê của người dùng, yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// 
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("v1")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> GetStatistics()
        {
            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
            var result = await _service.GetStatisticsAsync(userId);
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get statistic user successfully", result));
        }
        
        /// <summary>
        /// Lấy thống kê trên trang dashbaord, yêu cầu đăng nhập dưới quyền ADMIN
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/total")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseResponse<GetTotalStatisticResponse>>> GetTotalStatistic()
        {
            var result = await _service.GetTotalStatistic();
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get total statistic successfully", result));
        }
        
        /// <summary>
        /// Lấy thống kê người dùng online, yêu cầu đăng nhập dưới quyền ADMIN
        /// </summary>
        /// <remarks>
        /// period:
        ///
        /// - ByDate = 1 (lấy theo 30 ngày gần nhất),
        /// - ByMonth = 2 (lấy theo tháng chỉ trong năm hiện tại),
        /// - ByYear = 3 (lấy theo 5 năm gần nhất)
        /// </remarks>
        /// <remarks></remarks>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/online-users")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseResponse<GetOnlineUserStatisticResponse>>>
            GetOnlineUserStatistic(GetOnlineUserFilterOption filter)
        {
            var result = await _service.GetOnlineUserStatistic(filter);
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get online user statistic successfully", result));
        }
        
        /// <summary>
        /// Lấy thống kê số lượng bài thơ được tải lên, yêu cầu đăng nhập dưới quyền ADMIN
        /// </summary>
        /// <remarks>
        /// period:
        ///
        /// - ByDate = 1 (lấy theo 30 ngày gần nhất),
        /// - ByMonth = 2 (lấy theo tháng chỉ trong năm hiện tại),
        /// - ByYear = 3 (lấy theo 5 năm gần nhất)
        /// </remarks>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/poem-uploads")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseResponse<GetPoemUploadStatisticResponse>>>
            GetUploadPoemStatistic(GetPoemUploadFilterOption filter)
        {
            var result = await _service.GetUploadPoemStatistic(filter);
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get upload poem statistic successfully", result));
        }
        
        /// <summary>
        /// Lấy thống kê số lượng bài thơ theo từng loại, yêu cầu đăng nhập dưới quyền ADMIN
        /// </summary>
        /// <remarks>
        /// Type:
        ///
        /// - ThoTuDo = 1,
        /// - ThoLucBat = 2,
        /// - ThoSongThatLucBat = 3,
        /// - ThoThatNgonTuTuyet = 4,
        /// - ThoNguNgonTuTuyet = 5,
        /// - ThoThatNgonBatCu = 6,
        /// - ThoBonChu = 7,
        /// - ThoNamChu = 8,
        /// - ThoSauChu = 9,
        /// - ThoBayChu = 10,
        /// - ThoTamChu = 11,
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/poem-types")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseResponse<GetPoemTypeStatisticResponse>>> GetPoemTypeStatistic()
        {
            var result = await _service.GetPoemTypeStatistic();
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get poem type statistic successfully", result));
        }
    }
}

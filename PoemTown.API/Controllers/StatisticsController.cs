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
    }
}

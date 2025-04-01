using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UsageResponse;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;
using PoemTown.Service.QueryOptions.FilterOptions.UsageRightFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemSorts;
using PoemTown.Service.QueryOptions.SortOptions.UsageRightSorts;

namespace PoemTown.API.Controllers
{
    public class UsageRightsController : BaseController
    {
        private readonly IUsageRightService _service;
        private readonly IMapper _mapper;
        public UsageRightsController(IUsageRightService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Lấy danh sách bản thơ đã bán, yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/sold-poem")]
        public async Task<ActionResult<BaseResponse<GetSoldPoemResponse>>> GetSoldPoem(RequestOptionsBase<GetUsageRightPoemFilter, GetUsageRightPoemSort> request)
        {
            var userClaim = User.Claims.FirstOrDefault(p => p.Type == "UserId");
            Guid? userId = null;
            if (userClaim != null)
            {
                userId = Guid.Parse(userClaim.Value);
            }
            var paginationResponse = await _service.GetSoldPoem(userId.Value, request);
            var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetSoldPoemResponse>>(paginationResponse);
            basePaginationResponse.StatusCode = StatusCodes.Status200OK;
            basePaginationResponse.Message = "Get sold poem successfully";

            return Ok(basePaginationResponse);
        }

        /// <summary>
        /// Lấy danh sách bản thơ đã mua, yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/bought-poem")]
        public async Task<ActionResult<BaseResponse<GetBoughtPoemResponse>>> GetBoughtPoem(RequestOptionsBase<GetUsageRightPoemFilter, GetUsageRightPoemSort> request)
        {
            var userClaim = User.Claims.FirstOrDefault(p => p.Type == "UserId");
            Guid? userId = null;
            if (userClaim != null)
            {
                userId = Guid.Parse(userClaim.Value);
            }
            var paginationResponse = await _service.GetBoughtPoem(userId.Value, request);
            var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetBoughtPoemResponse>>(paginationResponse);
            basePaginationResponse.StatusCode = StatusCodes.Status200OK;
            basePaginationResponse.Message = "Get bought poem successfully";

            return Ok(basePaginationResponse);
        }
    }
}

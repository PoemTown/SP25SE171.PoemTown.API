using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemHistoryResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.PoemHistoryFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemHistorySorts;

namespace PoemTown.API.Controllers;

public class PoemHistoriesController : BaseController
{
    private readonly IPoemHistoryService _poemHistoryService;
    private readonly IMapper _mapper;
    public PoemHistoriesController(IPoemHistoryService poemHistoryService, IMapper mapper)
    {
        _poemHistoryService = poemHistoryService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Lấy lịch sử bài thơ, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// CHÚ Ý REQUEST PARAMETER:
    ///
    /// - poemId: Lấy từ request path
    ///
    /// Type: Loại bài thơ, thể thơ:
    ///
    /// SortOptions:
    ///
    /// - CreatedTimeAscending = 1 (Thời gian tạo tăng dần - tức là cũ nhất -> mới nhất),
    /// - CreatedTimeDescending = 2 (Thời gian tạo giảm dần - tức là mới nhất -> cũ nhất) (Mặc định),
    /// </remarks>
    /// <param name="poemId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/{poemId}")]
    [Authorize]
    public async Task<ActionResult<BasePaginationResponse<GetPoemHistoryResponse>>>
        GetPoemHistories(Guid poemId, RequestOptionsBase<GetPoemHistoryFilterOption, GetPoemHistorySortOptions> request)
    {
        var paginationResponse = await _poemHistoryService.GetPoemHistories(poemId, request);
        
        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetPoemHistoryResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get poems histories successfully";
        
        return Ok(basePaginationResponse);
    }

}
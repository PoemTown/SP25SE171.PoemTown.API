using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
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
    /// Status: Trạng thái của bài thơ, mặc định là Draft (hiện tại chỉ sử dụng để hiển thị trên giao diện, Status không có ý nghĩa gì trong lịch sử chỉnh sửa của bài thơ)
    ///
    /// 0: Draft (Nháp)
    /// 1: Posted (Đã đăng)
    /// 2: Suspended (Bị đinh chỉ)
    /// 
    /// Type: Loại bài thơ, thể thơ:
    ///
    /// SortOptions:
    ///
    /// - CreatedTimeAscending = 1 (Thời gian tạo tăng dần - tức là cũ nhất -> mới nhất),
    /// - CreatedTimeDescending = 2 (Thời gian tạo giảm dần - tức là mới nhất -> cũ nhất) (Mặc định),
    /// </remarks>
    /// <param name="poemId">Lấy từ request path</param>
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

    /// <summary>
    /// Lấy chi tiết lịch sử chỉnh sửa của bài thơ, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// Status: Trạng thái của bài thơ, mặc định là Draft (hiện tại chỉ sử dụng để hiển thị trên giao diện, Status không có ý nghĩa gì trong lịch sử chỉnh sửa của bài thơ)
    ///
    /// 0: Draft (Nháp)
    /// 1: Posted (Đã đăng)
    /// 2: Suspended (Bị đinh chỉ)
    /// 
    /// Type: Loại bài thơ, thể thơ:
    ///
    /// </remarks>
    /// <param name="poemHistoryId">Lấy từ request path</param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/detail/{poemHistoryId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<GetPoemHistoryDetailResponse>>> GetPoemHistoryDetail(Guid poemHistoryId)
    {
        var poemHistory = await _poemHistoryService.GetPoemHistoryDetail(poemHistoryId);
        
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Get poem history detail successfully", poemHistory));
    }
    
    /// <summary>
    /// Xóa lịch sử chỉnh sửa bài thơ, yêu cầu đăng nhập
    /// </summary>
    /// <param name="poemHistoryId">Lấy từ request path</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{poemHistoryId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DeletePoemHistory(Guid poemHistoryId)
    {
        await _poemHistoryService.DeletePoemHistory(poemHistoryId);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Poem history deleted successfully"));
    }
    
    /// <summary>
    /// Xóa lịch sử chỉnh sửa bài thơ (vĩnh viễn), yêu cầu đăng nhập
    /// </summary>
    /// <param name="poemHistoryId">Lấy từ request path</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{poemHistoryId}/permanent")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DeletePoemHistoryPermanent(Guid poemHistoryId)
    {
        await _poemHistoryService.DeletePoemHistoryPermanent(poemHistoryId);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Poem history deleted permanently successfully"));
    }
    
    /// <summary>
    /// Xóa nhiều lịch sử chỉnh sửa bài thơ, yêu cầu đăng nhập
    /// </summary>
    /// <param name="poemHistoryIds">Lấy từ request query</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/list/")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DeletePoemHistories([FromQuery] IEnumerable<Guid> poemHistoryIds)
    {
        await _poemHistoryService.DeletePoemHistories(poemHistoryIds);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Poem histories deleted successfully"));
    }
    
    /// <summary>
    /// Xóa nhiều lịch sử chỉnh sửa bài thơ (vĩnh viễn), yêu cầu đăng nhập
    /// </summary>
    /// <param name="poemHistoryIds">Lấy từ request query</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/list/permanent")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DeletePoemHistoriesPermanent([FromQuery] IEnumerable<Guid> poemHistoryIds)
    {
        await _poemHistoryService.DeletePoemHistoriesPermanent(poemHistoryIds);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Poem histories deleted permanently successfully"));
    }
    
}
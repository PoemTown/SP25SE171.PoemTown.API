using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.DailyMessageRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.DailyMessageResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.DailyMessageFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.DailyMessageSorts;

namespace PoemTown.API.Controllers;

public class DailyMessagesController : BaseController
{
    private readonly IDailyMessageService _dailyMessageService;
    private readonly IMapper _mapper;
    public DailyMessagesController(IDailyMessageService dailyMessageService, IMapper mapper)
    {
        _dailyMessageService = dailyMessageService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Tạo mới một thông điệp hằng ngày, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> CreateNewDailyMessage(CreateNewDailyMessageRequest request)
    {
        await _dailyMessageService.CreateNewDailyMessage(request);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Create new daily message successfully"));
    }
    
    /// <summary>
    /// Cập nhật một thông điệp hằng ngày, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> UpdateDailyMessage(UpdateDailyMessageRequest request)
    {
        await _dailyMessageService.UpdateDailyMessage(request);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Update daily message successfully"));
    }
    
    /// <summary>
    /// Xóa một thông điệp hằng ngày, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="dailyMessageId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{dailyMessageId}")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> DeleteDailyMessage(Guid dailyMessageId)
    {
        await _dailyMessageService.DeleteDailyMessage(dailyMessageId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Delete daily message successfully"));
    }
    
    /// <summary>
    /// Lấy thông điệp hằng ngày theo Id, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="dailyMessageId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/{dailyMessageId}")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse<GetDailyMessageResponse>>> GetDailyMessage(Guid dailyMessageId)
    {
        var response = await _dailyMessageService.GetDailyMessage(dailyMessageId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Get daily message successfully", response));
    }
    
    /// <summary>
    /// Lấy thông điệp hằng ngày đang được sử dụng, không yêu cầu đăng nhập
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/in-use")]
    public async Task<ActionResult<BaseResponse<GetDailyMessageResponse>>> GetInUseDailyMessage()
    {
        var response = await _dailyMessageService.GetInUseDailyMessage();
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Get in use daily message successfully", response));
    }
    
    /// <summary>
    /// Lấy danh sách thông điệp hằng ngày, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// sortOptions:
    ///
    /// 1. CreatedTimeAscending
    /// 2. CreatedTimeDescending
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BasePaginationResponse<GetDailyMessageResponse>>> GetDailyMessages(
        [FromQuery] RequestOptionsBase<GetDailyMessageFilterOption, GetDailyMessageSortOption> request)
    {
        var paginationResponse = await _dailyMessageService.GetDailyMessages(request);

        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetDailyMessageResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get daily messages successfully";
        
        return Ok(basePaginationResponse);
    }
}
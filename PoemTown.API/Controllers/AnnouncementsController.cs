using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.AnnouncementRequests;
using PoemTown.Service.BusinessModels.ResponseModels.AnnouncementResponses;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.AnnouncementFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.AnnouncementSorts;

namespace PoemTown.API.Controllers;

public class AnnouncementsController : BaseController
{
    private readonly IAnnouncementService _announcementService;
    private readonly IMapper _mapper;
    
    public AnnouncementsController(IAnnouncementService announcementService,
        IMapper mapper)
    {
        _announcementService = announcementService;
        _mapper = mapper;
    }
    
    [HttpPost]
    [Route("v1")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> CreateAnnouncement([FromBody] CreateNewAnnouncementRequest request)
    {
        
        await _announcementService.SendAnnouncementAsync(request);
        return Created(String.Empty, new BaseResponse(StatusCodes.Status201Created, "Created successfully"));
    }
    
    /// <summary>
    /// Gửi thông báo từ hệ thống đến tất cả người dùng, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/admin")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> CreateAnnouncementToUser([FromBody] CreateNewAnnouncementRequest request)
    {
        await _announcementService.AdminSendAnnouncementAsync(request);
        return Created(String.Empty, new BaseResponse(StatusCodes.Status201Created, "Created successfully"));
    }
    
    /// <summary>
    /// Lấy danh sách thông báo của tôi, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// type:
    ///
    /// - Like = 1,
    /// - Comment = 2,
    /// - User = 3,
    /// - Report = 4,
    /// - Collection = 5,
    /// - Poem = 6,
    /// - Transaction = 7,
    /// - Achievement = 8,
    /// - PoemLeaderboard = 9,
    /// - UserLeaderboard = 10,
    /// - RecordFile = 11,
    /// - Follower = 12,
    ///
    /// sortOptions:
    ///
    /// - CreatedtimeAscending = 1,
    /// - CreatedtimeDescending = 2,
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/mine")]
    [Authorize]
    public async Task<ActionResult<BasePaginationResponse<GetAnnouncementResponse>>>
        GetUserAnnouncements(RequestOptionsBase<GetAnnouncementFilterOption, GetAnnouncementSortOption> request)
    {
        Guid userId = Guid.Parse(User.Claims.First(p => p.Type == "UserId").Value);
     
        var paginationResponse = await _announcementService.GetUserAnnouncementsAsync(userId, request);
        
        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetAnnouncementResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get user announcements successfully";

        return basePaginationResponse;
    }
    
    /// <summary>
    /// Lấy danh sách thông báo hệ thống, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/system")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BasePaginationResponse<GetAnnouncementResponse>>>
        GetSystemAnnouncements(RequestOptionsBase<GetAnnouncementFilterOption, GetAnnouncementSortOption> request)
    {
        var paginationResponse = await _announcementService.GetSystemAnnouncements(request);
        
        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetAnnouncementResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get system announcements successfully";

        return basePaginationResponse;
    }
    
    /// <summary>
    /// Cập nhật thông báo thành đã đọc, yêu cầu đăng nhập
    /// </summary>
    /// <param name="announcementId"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/{announcementId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> UpdateUserAnnouncementToRead(Guid announcementId)
    {
        Guid userId = Guid.Parse(User.Claims.First(p => p.Type == "UserId").Value);
        await _announcementService.UpdateAnnouncementToRead(userId, announcementId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Update announcement successfully"));
    }
    
    /// <summary>
    /// Xóa thông báo của tôi, yêu cầu đăng nhập
    /// </summary>
    /// <param name="announcementId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/mine/{announcementId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DeleteUserAnnouncement(Guid announcementId)
    {
        Guid userId = Guid.Parse(User.Claims.First(p => p.Type == "UserId").Value);
        await _announcementService.DeleteAnnouncementAsync(userId, announcementId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Delete announcement successfully"));
    }
    
    /// <summary>
    /// Xóa tất cả thông báo của tôi, yêu cầu đăng nhập
    /// </summary>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/mine/all")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DeleteAllUserAnnouncements()
    {
        Guid userId = Guid.Parse(User.Claims.First(p => p.Type == "UserId").Value);
        await _announcementService.DeleteAllUserAnnouncementsAsync(userId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Delete all announcements successfully"));
    }
}
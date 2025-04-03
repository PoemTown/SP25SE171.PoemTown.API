using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.AnnouncementRequests;
using PoemTown.Service.BusinessModels.ResponseModels.AnnouncementResponses;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.Interfaces;

namespace PoemTown.API.Controllers;

public class AnnouncementsController : BaseController
{
    private readonly IAnnouncementService _announcementService;
    
    public AnnouncementsController(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
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
    /// Lấy danh sách thông báo của tôi, yêu cầu đăng nhập
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/mine")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<IEnumerable<GetAnnouncementResponse>>>> GetUserAnnouncements()
    {
        Guid userId = Guid.Parse(User.Claims.First(p => p.Type == "UserId").Value);
     
        var announcements = await _announcementService.GetUserAnnouncementsAsync(userId);
        return Ok(new BaseResponse<IEnumerable<GetAnnouncementResponse>>(StatusCodes.Status200OK, "Get user announcement successfully" ,announcements));
    }
    
    /// <summary>
    /// Cập nhật thông báo thành đã đọc, yêu cầu đăng nhập
    /// </summary>
    /// <param name="announcementId"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/{announcementId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> UpdateAnnouncementToRead(Guid announcementId)
    {
        await _announcementService.UpdateAnnouncementToRead(announcementId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Update announcement successfully"));
    }
}
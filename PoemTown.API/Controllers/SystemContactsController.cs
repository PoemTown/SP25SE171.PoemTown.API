using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.SystemContactRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.SystemContactResponses;
using PoemTown.Service.Interfaces;

namespace PoemTown.API.Controllers;

public class SystemContactsController : BaseController
{
    private readonly ISystemContactService _systemContactService;
    public SystemContactsController(ISystemContactService systemContactService)
    {
        _systemContactService = systemContactService;
    }
    
    /// <summary>
    /// Tạo mới thông tin liên lạc của hệ thống PoemTown, yêu câu đăng nhập dươới quyền Admin
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> CreateNewSystemContact(CreateNewSystemContactRequest request)
    {
        await _systemContactService.CreateNewSystemContact(request);
        return Created(String.Empty, new BaseResponse(StatusCodes.Status201Created, "Create new system contact successfully" ));
    }
    
    /// <summary>
    /// Cập nhật thông tin liên lạc của hệ thống PoemTown, yêu câu đăng nhập dươới quyền Admin
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> UpdateSystemContact(UpdateSystemContactRequest request)
    {
        await _systemContactService.UpdateSystemContact(request);
        return Accepted(new BaseResponse(StatusCodes.Status202Accepted, "Update system contact successfully"));
    }
    
    /// <summary>
    /// Xóa thông tin liên lạc của hệ thống PoemTown, yêu câu đăng nhập dươới quyền Admin
    /// </summary>
    /// <param name="systemContactId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{systemContactId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> DeleteSystemContact(Guid systemContactId)
    {
        await _systemContactService.DeleteSystemContact(systemContactId);
        return Accepted(new BaseResponse(StatusCodes.Status202Accepted, "Delete system contact successfully"));
    }
    
    /// <summary>
    /// Lấy thông tin liên lạc của hệ thống PoemTown, không yêu cầu đăng nhập
    /// </summary>
    /// <param name="systemContactId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/{systemContactId}")]
    public async Task<ActionResult<BaseResponse<GetSystemContactResponse>>> GetSystemContactById(Guid systemContactId)
    {
        var response = await _systemContactService.GetSystemContactById(systemContactId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Get system contact successfully", response));
    }
    
    /// <summary>
    /// Lấy tất cả thông tin liên lạc của hệ thống PoemTown, không yêu cầu đăng nhập
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("v1")]
    public async Task<ActionResult<IEnumerable<GetSystemContactResponse>>> GetAllSystemContacts()
    {
        var response = await _systemContactService.GetAllSystemContacts();
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Get all system contacts successfully", response));
    }
    
    /// <summary>
    /// Tải lên icon cho thông tin liên lạc của hệ thống PoemTown, yêu cầu đăng nhập dưới quyền Admin
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/icon")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> UploadSystemContactIcon(IFormFile file)
    {
        var response = await _systemContactService.UploadSystemContactIcon(file);
        return Created(String.Empty, new BaseResponse(StatusCodes.Status201Created, "Upload system contact icon successfully", response));
    }
    
}
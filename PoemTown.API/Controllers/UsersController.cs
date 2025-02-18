using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.UserRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Interfaces;

namespace PoemTown.API.Controllers;

public class UsersController : BaseController
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    /// <summary>
    /// Người dùng lấy thông tin cá nhân của mình, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// Status: trạng thái của tài khoản
    ///
    /// Active = 1,
    /// InActive = 2,
    /// Locked = 3,
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/mine/profile")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<GetUserProfileResponse>>> GetMyProfile()
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        var response = await _userService.GetMyProfile(userId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "User profile retrieved successfully", response));
    }
    
    /// <summary>
    /// Cập nhật thông tin cá nhân của người dùng, yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/mine/profile")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> UpdateMyProfile(UpdateMyProfileRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _userService.UpdateMyProfile(userId, request);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "User profile updated successfully"));
    }
}
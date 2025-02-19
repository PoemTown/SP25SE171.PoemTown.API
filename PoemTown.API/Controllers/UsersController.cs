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
    
    /// <summary>
    /// Upload ảnh đại diện của người dùng, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// Upload ảnh, sau đó trả ra image url, copy url đó vào trường Avatar trong API "Update: v1/mine/profile" để cập nhật ảnh đại diện
    /// </remarks>
    /// <param name="file">lấy từ form data</param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/mine/profile/image")]    
    [Authorize]
    public async Task<ActionResult<BaseResponse<string>>> UploadProfileImage(IFormFile file)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        var response = await _userService.UploadProfileImage(userId, file);
        return Ok(new BaseResponse<string>(StatusCodes.Status201Created, "Profile image uploaded successfully", response));
    }
    
    /// <summary>
    /// Lấy thông tin cá nhân của người dùng online (trên thanh header), yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// - TotalFollowers: tổng số người đang theo dõi tôi
    ///
    /// - TotalFollowings: tổng số người tôi đang theo dõi
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/mine/profile/online")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<GetOwnOnlineProfileResponse>>> GetOwnOnlineProfile()
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        var response = await _userService.GetOwnOnlineProfile(userId);
        return Ok(new BaseResponse<GetOwnOnlineProfileResponse>(StatusCodes.Status200OK, "User online profile retrieved successfully", response));
    }
}
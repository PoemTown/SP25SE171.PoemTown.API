using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Repository.Enums.Accounts;
using PoemTown.Service.BusinessModels.RequestModels.AccountRequests;
using PoemTown.Service.BusinessModels.ResponseModels.AccountResponses;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.AccountFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.AccountSorts;

namespace PoemTown.API.Controllers;

public class AccountsController : BaseController
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;
    public AccountsController(IAccountService accountService, IMapper mapper)
    {
        _accountService = accountService;
        _mapper = mapper;
    }
      
    /// <summary>
    /// Confirm email by email otp
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/email/confirmation")]
    public async Task<ActionResult<BaseResponse>> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        await _accountService.ConfirmEmail(request);
        return Accepted(String.Empty, new BaseResponse(StatusCodes.Status202Accepted, "Email confirmed"));
    }
    
    /// <summary>
    /// Send email otp to user email
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/email/otp")]
    public async Task<ActionResult<BaseResponse>> SendEmailOtp([FromBody] ResendEmailConfirmationRequest request)
    {
        await _accountService.SendEmailOtp(request);
        return Accepted(String.Empty, new BaseResponse(StatusCodes.Status202Accepted, "Email OTP sent"));
    }
    
    /// <summary>
    /// Change password
    /// </summary>
    /// <remarks>
    /// Require authentication to take email from Bearer token
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/password")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _accountService.ChangePassword(userId, request);
        return Accepted(String.Empty, new BaseResponse(StatusCodes.Status202Accepted, "Password changed"));
    }
    
    /// <summary>
    /// Sending email reset password token to user email (as redirect link) (Safe method)
    /// </summary>
    /// <param name="request"></param>
    [HttpPut]
    [Route("v1/password/recovery")]
    public async Task<ActionResult<BaseResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        await _accountService.ForgotPassword(request);
        return Accepted(String.Empty, new BaseResponse(StatusCodes.Status202Accepted, "Forgot password accepted"));
    }
    
    /// <summary>
    /// Create new password when forgot password (Safe method)
    /// </summary>
    /// <param name="request">
    /// Data from reset password url return including:
    /// - email
    /// - token
    /// - timestamp
    ///
    /// Data request from body:
    /// - email: email,
    /// - resetPasswordToken: token,
    /// - expiredTimeStamp: timestamp
    /// - newPassword: take from user input form
    /// </param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/password/recovery")]
    public async Task<ActionResult<BaseResponse>> NewPasswordForgot([FromBody] NewPasswordForgotRequest request)
    {
        await _accountService.NewPasswordForgot(request);
        return Created(String.Empty, new BaseResponse(StatusCodes.Status201Created, "New password created"));
    }
    
    /// <summary>
    /// Lấy danh sách tài khoản, yêu cầu quyền ADMIN
    /// </summary>
    /// <remarks>
    /// Account Status:
    /// - Active = 1,
    /// - InActive = 2,
    /// - Locked = 3,
    /// 
    /// SortOptions:
    /// 
    /// - CreatedTimeAscending = 1,
    /// - CreatedTimeDescending = 2,
    /// - LastUpdatedTimeAscending = 3,
    /// - LastUpdatedTimeDescending = 4,
    /// - DeletedTimeAscending = 5,
    /// - DeletedTimeDescending = 6
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/accounts")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BasePaginationResponse<GetAccountResponse>>>
        GetAccounts(RequestOptionsBase<GetAccountFilterOption, GetAccountSortOption> request)
    {
        var paginationResponse = await _accountService.GetAccounts(request);

        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetAccountResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get accounts successfully";
        
        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Lấy chi tiết tài khoản, yêu cầu quyền ADMIN
    /// </summary>
    /// <remarks>
    /// Account Status:
    ///
    /// - Active = 1,
    /// - InActive = 2,
    /// - Locked = 3,
    /// </remarks>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/accounts/detail/{userId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse<GetAccountDetailResponse>>>
        GetAccountDetail(Guid userId)
    {
        var accountDetail = await _accountService.GetAccountDetail(userId);
        return Ok(new BaseResponse<GetAccountDetailResponse>(StatusCodes.Status200OK, "Account detail retrieved", accountDetail));
    }
    
    /// <summary>
    /// Cập nhật trạng thái tài khoản, yêu cầu quyền ADMIN
    /// </summary>
    /// <remarks>
    /// Account Status:
    ///
    /// - Active = 1,
    /// - InActive = 2,
    /// - Locked = 3,
    /// </remarks>
    /// <param name="userId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/accounts/status/{userId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> UpdateAccountStatus(Guid userId, [FromQuery] AccountStatus status)
    {
        await _accountService.UpdateAccountStatus(userId, status);
        return Accepted(String.Empty, new BaseResponse(StatusCodes.Status202Accepted, "Account status updated"));
    }
    
    /// <summary>
    /// Cập nhật quyền hạn của tài khoản, yêu cầu quyền ADMIN
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleId"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/accounts/role/{userId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> UpdateAccountRole(Guid userId, [FromQuery] Guid roleId)
    {
        await _accountService.AddAccountRole(userId, roleId);
        return Accepted(String.Empty, new BaseResponse(StatusCodes.Status202Accepted, "Account role added"));
    }
    
    /// <summary>
    /// Xóa quyền hạn của tài khoản, yêu cầu quyền ADMIN
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/accounts/role/{userId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> RemoveAccountRole(Guid userId, [FromQuery] Guid roleId)
    {
        await _accountService.RemoveAccountRole(userId, roleId);
        return Accepted(String.Empty, new BaseResponse(StatusCodes.Status202Accepted, "Account role removed"));
    }
}
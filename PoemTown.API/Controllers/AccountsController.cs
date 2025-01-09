using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.AccountRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.Interfaces;

namespace PoemTown.API.Controllers;

public class AccountsController : BaseController
{
    private readonly IAccountService _accountService;
    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
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
}
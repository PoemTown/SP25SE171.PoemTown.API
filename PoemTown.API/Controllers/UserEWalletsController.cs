using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.UserEWalletRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.UserEWalletResponses;
using PoemTown.Service.Interfaces;

namespace PoemTown.API.Controllers;

public class UserEWalletsController : BaseController
{
    private readonly IUserEWalletService _userEWalletService;
    
    public UserEWalletsController(IUserEWalletService userEWalletService)
    {
        _userEWalletService = userEWalletService;
    }
    
    /// <summary>
    /// Nạp tiền vào ví điện tử của người dùng, yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/deposit")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<DepositUserEWalletResponse>>> DepositUserEWalletAsync(DepositUserEWalletRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value);
        
        var response = await _userEWalletService.DepositUserEWalletAsync(userId, request);
        return Ok(response);
    }
    
    /// <summary>
    /// Quyên tặng tiền vào ví điện tử người dùng khác, yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/donate")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DonateUserEWalletAsync(DonateUserEWalletRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value);
        
        await _userEWalletService.DonateUserEWalletAsync(userId, request);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Donate successfully"));
    }
}
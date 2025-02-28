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
}
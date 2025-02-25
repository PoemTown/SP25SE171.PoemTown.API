using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.UserEWalletRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.UserEWalletResponses;
using PoemTown.Service.Interfaces;

namespace PoemTown.API.Controllers;

public class UserEWalletController : BaseController
{
    private readonly IUserEWalletService _userEWalletService;
    
    public UserEWalletController(IUserEWalletService userEWalletService)
    {
        _userEWalletService = userEWalletService;
    }
    
    [HttpPost]
    [Route("v1/deposit")]
    public async Task<ActionResult<BaseResponse<DepositUserEWalletResponse>>> DepositUserEWalletAsync(DepositUserEWalletRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value);
        
        var response = await _userEWalletService.DepositUserEWalletAsync(userId, request);
        return Ok(response);
    }
}
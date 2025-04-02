using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.CustomAttribute;

namespace PoemTown.API.Base;

[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(ValidateModelStateAttribute))]
public class BaseController : ControllerBase
{
    public Guid? GetNullableUserId()
    {
        var userClaim = User.Claims.FirstOrDefault(p => p.Type == "UserId");
        Guid? userId = null;
        if (userClaim != null)
        {
            userId = Guid.Parse(userClaim.Value);
        }

        return userId;
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.Interfaces;

namespace PoemTown.API.Controllers;

public class PoemsController : BaseController
{
    private readonly IPoemService _poemService;
    public PoemsController(IPoemService poemService)
    {
        _poemService = poemService;
    }

    [HttpPost]
    [Route("v1")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> CreateNewPoem(CreateNewPoemRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _poemService.CreateNewPoem(userId, request);
        return Ok(new BaseResponse(StatusCodes.Status201Created, "Poem created successfully"));
    }
}
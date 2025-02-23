using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.Interfaces;

namespace PoemTown.API.Controllers;

public class LikesController : BaseController
{
    private readonly ILikeService _likeService;
    public LikesController(ILikeService likeService)
    {
        _likeService = likeService;
    }
    
    /// <summary>
    /// Thích một bài thơ, yêu cầu đăng nhập
    /// </summary>
    /// <param name="poemId">Lấy từ request path</param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/{poemId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> LikePoem(Guid poemId)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _likeService.LikePoem(userId, poemId);
        return Ok(new BaseResponse(StatusCodes.Status201Created, "Poem liked successfully"));
    }
    
    /// <summary>
    /// Bỏ thích một bài thơ, yêu cầu đăng nhập
    /// </summary>
    /// <param name="poemId">Lấy từ request path</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{poemId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DislikePoem(Guid poemId)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _likeService.DislikePoem(userId, poemId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Poem disliked successfully"));
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.ContentPageRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.ContenPageResponses;
using PoemTown.Service.Interfaces;

namespace PoemTown.API.Controllers;

public class ContentPagesController : BaseController
{
    private readonly IContentPageService _contentPageService;
    public ContentPagesController(IContentPageService contentPageService)
    {
        _contentPageService = contentPageService;
    }
    
    /// <summary>
    /// Tạo mới nội dung cho trang About PoemTown, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> CreateNewContentPage(CreateNewContentPageRequest request)
    {
        await _contentPageService.CreateNewContentPage(request);
        return Created(String.Empty, new BaseResponse(StatusCodes.Status201Created, "Content page created successfully"));
    }
    
    /// <summary>
    /// Cập nhật nội dung cho trang About PoemTown, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> UpdateContentPage(UpdateContentPageRequest request)
    {
        await _contentPageService.UpdateContentPage(request);
        return Accepted(new BaseResponse(StatusCodes.Status202Accepted, "Content page updated successfully"));
    }
    
    /// <summary>
    /// Xóa nội dung cho trang About PoemTown, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="contentPageId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{contentPageId}")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> DeleteContentPage(Guid contentPageId)
    {
        await _contentPageService.DeleteContentPage(contentPageId);
        return Accepted(new BaseResponse(StatusCodes.Status202Accepted, "Content page deleted successfully"));
    }
    
    /// <summary>
    /// Xóa vĩnh viễn nội dung cho trang About PoemTown, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="contentPageId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/permanent/{contentPageId}")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> DeleteContentPagePermanently(Guid contentPageId)
    {
        await _contentPageService.DeleteContentPagePermanently(contentPageId);
        return Accepted(new BaseResponse(StatusCodes.Status202Accepted, "Content page deleted permanently successfully"));
    }
    
    /// <summary>
    /// Lấy nội dung chi tiết cho trang About PoemTown, không yêu cầu đăng nhập
    /// </summary>
    /// <param name="contentPageId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/{contentPageId}")]
    public async Task<ActionResult<BaseResponse<GetContentPageResponse>>> GetContentPage(Guid contentPageId)
    {
        var contentPage = await _contentPageService.GetContentPage(contentPageId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Get content page successfully", contentPage));
    }
    
    /// <summary>
    /// Lấy danh sách nội dung cho trang About PoemTown, không yêu cầu đăng nhập
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("v1")]
    public async Task<ActionResult<BaseResponse<IEnumerable<GetContentPageResponse>>>> GetContentPages()
    {
        var contentPages = await _contentPageService.GetContentPages();
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Get content pages successfully", contentPages));
    }
}
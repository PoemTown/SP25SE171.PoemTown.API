using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.CommentRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.CommentResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.CommentFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.CommentSorts;

namespace PoemTown.API.Controllers;

public class CommentsController : BaseController
{
    private readonly ICommentService _commentService;
    private readonly IMapper _mapper;
    public CommentsController(ICommentService commentService, IMapper mapper)
    {
        _commentService = commentService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Bình luận vào một bài thơ, yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> CommentPoem(CommentPoemRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _commentService.CommentPoem(userId, request);
        return Ok(new BaseResponse(StatusCodes.Status201Created, "Comment poem successfully"));
    }
    
    /// <summary>
    /// Phản hồi lại một bình luận, yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/respondent")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> ReplyComment(ReplyCommentRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _commentService.ReplyComment(userId, request);
        return Ok(new BaseResponse(StatusCodes.Status201Created, "Reply comment successfully"));
    }
    
    /// <summary>
    /// Xóa một bình luận (vĩnh viễn), yêu cầu đăng nhập
    /// </summary>
    /// <param name="commentId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{commentId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DeleteCommentPermanent(Guid commentId)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _commentService.DeleteCommentPermanent(userId, commentId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Delete comment successfully"));
    }
    
    /// <summary>
    /// Lấy danh sách bình luận của một bài thơ, không yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// Để lấy được paging vượt mức 250 (tức pageSize > 250), set allowExceedPageSize = true
    /// 
    /// CHÚ Ý REQUEST PARAMETER:
    ///
    /// poemId: lấy từ request path
    ///
    /// SortOptions:
    ///
    /// - CreatedTimeAscending = 1 (Thời gian tạo tăng dần),
    /// - CreatedTimeDescending = 2 (Thời gian tạo giảm dần) (Mặc định)
    /// </remarks>
    /// <param name="poemId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/{poemId}")]
    public async Task<ActionResult<PaginationResponse<GetCommentResponse>>>
        GetPostComments(Guid poemId, RequestOptionsBase<GetPostCommentFilterOption, GetPostCommentSortOption> request)
    {
        var userClaim = User.Claims.FirstOrDefault(p => p.Type == "UserId");
        Guid? userId = null;
        if (userClaim != null)
        {
            userId = Guid.Parse(userClaim.Value);
        }   
        
        var paginationResponse = await _commentService.GetPostComments(userId, poemId, request);

        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetCommentResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get comments successfully";
        
        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Xóa một bình luận (vĩnh viễn), yêu cầu đăng nhập dung quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="commentId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/admin/{commentId}")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> DeleteCommentPermanentByAdminAndModerator(Guid commentId)
    {
        await _commentService.DeleteCommentPermanentByAdminAndModerator(commentId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Xóa bình luận thành công!"));
    }
    
}
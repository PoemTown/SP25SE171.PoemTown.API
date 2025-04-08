using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.FollowerResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.FollowerFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.FollowerSorts;

namespace PoemTown.API.Controllers;

public class FollowersController : BaseController
{
    private readonly IFollowerService _followerService;
    private readonly IMapper _mapper;
    
    public FollowersController(IFollowerService followerService, IMapper mapper)
    {
        _followerService = followerService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Theo dõi một người dùng, yêu cầu đăng nhập
    /// </summary>
    /// <param name="followedUserId">userId của người được theo dõi, Lấy từ request path</param>
    /// <returns></returns>
    [HttpPost]
    [Route("{followedUserId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> FollowUserAsync(Guid followedUserId)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _followerService.FollowUserAsync(userId, followedUserId);
        return Ok(new BaseResponse(StatusCodes.Status201Created, "Followed user successfully"));
    }
    
    /// <summary>
    /// Bỏ theo dõi một người dùng, yêu cầu đăng nhập
    /// </summary>
    /// <param name="followerId">id của bảng follower, Lấy từ request path</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{followerId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> UnfollowUserAsync(Guid followerId)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _followerService.UnfollowUserAsync(userId, followerId);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Unfollowed user successfully"));
    }
    
    /// <summary>
    /// Lấy danh sách người dùng đang theo dõi mình, yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("mine")]
    [Authorize]
    public async Task<ActionResult<BasePaginationResponse<GetFollowersResponse>>>
        GetMyFollowers(RequestOptionsBase<GetFollowersFilterOption, GetFollowersSortOption> request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        
        var paginationResponse = await _followerService.GetMyFollower(userId, request);
        
        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetFollowersResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get my followers successfully";
        
        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Lấy danh sách người dùng mà mình đang theo dõi, yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("mine/follow-list")]
    [Authorize]
    public async Task<ActionResult<BasePaginationResponse<GetFollowersResponse>>>
        GetMyFollowList(RequestOptionsBase<GetFollowersFilterOption, GetFollowersSortOption> request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        
        var paginationResponse = await _followerService.GetMyFollowList(userId, request);
        
        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetFollowersResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get my follow list successfully";
        
        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Lấy danh sách mà người dùng hiện đang theo dõi những người dùng khác
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("user/{userName}/follow-list")]
    public async Task<ActionResult<BasePaginationResponse<GetFollowersResponse>>>
        GetUserFollowList(string userName,
            RequestOptionsBase<GetFollowersFilterOption, GetFollowersSortOption> request)
    {
        var paginationResponse = await _followerService.GetUserFollowList(userName, request);
        
        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetFollowersResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get user follow list successfully";
        
        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Lấy danh sách người dùng đang theo dõi một người dùng khác
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("user/{userName}")]
    public async Task<ActionResult<BasePaginationResponse<GetFollowersResponse>>>
        GetUserFollower(string userName,
            RequestOptionsBase<GetFollowersFilterOption, GetFollowersSortOption> request)
    {
        var paginationResponse = await _followerService.GetUserFollower(userName, request);
        
        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetFollowersResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get user follower successfully";
        
        return Ok(basePaginationResponse);
    }
}
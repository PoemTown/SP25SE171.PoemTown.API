using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.CollectionFilters;
using PoemTown.Service.QueryOptions.FilterOptions.TargetMarkFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.CollectionSorts;
using PoemTown.Service.QueryOptions.SortOptions.TargetMarkSorts;

namespace PoemTown.API.Controllers;

public class TargetMarksController : BaseController
{
    private readonly ITargetMarkService _targetMarkService;
    private readonly IMapper _mapper;

    public TargetMarksController(ITargetMarkService targetMarkService, IMapper mapper)
    {
        _targetMarkService = targetMarkService;
        _mapper = mapper;
    }

    /// <summary>
    /// Tạo một target mark cho một bài thơ, yêu cầu đăng nhập
    /// </summary>
    /// <param name="poemId">Lấy từ request path</param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/poem/{poemId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> TargetMarkPoem(Guid poemId)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _targetMarkService.TargetMarkPoem(poemId, userId);
        return Ok(new BaseResponse(StatusCodes.Status201Created, "Poem marked successfully"));
    }

    /// <summary>
    /// Bỏ target mark của một bài thơ, yêu cầu đăng nhập
    /// </summary>
    /// <param name="poemId">Lấy từ request path</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/poem/{poemId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> UnTargetMarkPoem(Guid poemId)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _targetMarkService.UnTargetMarkPoem(poemId, userId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Poem unmarked successfully"));
    }

    /// <summary>
    /// Tạo một target mark cho một bộ sưu tập, yêu cầu đăng nhập
    /// </summary>
    /// <param name="collectionId">Lấy từ request path</param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/collection/{collectionId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> TargetMarkCollection(Guid collectionId)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _targetMarkService.TargetMarkCollection(collectionId, userId);
        return Ok(new BaseResponse(StatusCodes.Status201Created, "Collection marked successfully"));
    }

    /// <summary>
    /// Bỏ target mark của một bộ sưu tập, yêu cầu đăng nhập
    /// </summary>
    /// <param name="collectionId">Lấy từ request path</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/collection/{collectionId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> UnTargetMarkCollection(Guid collectionId)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _targetMarkService.UnTargetMarkCollection(collectionId, userId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Collection unmarked successfully"));
    }

    /// <summary>
    /// Lấy danh sách bài thơ đã được target mark, yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/poem")]
    [Authorize]
    public async Task<ActionResult<BasePaginationResponse<GetPoemInTargetMarkResponse>>> GetPoemInTargetMark(
        RequestOptionsBase<GetPoemInTargetMarkFilterOption, GetPoemInTargetMarkSortOption> request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);

        var paginationResponse = await _targetMarkService.GetPoemInTargetMark(userId, request);

        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetPoemInTargetMarkResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get poems in target mark successfully";

        return Ok(basePaginationResponse);
    }

    /// <summary>
    /// Lấy danh sách bộ sưu tập đã được target mark, yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/collection")]
    [Authorize]
    public async Task<ActionResult<BasePaginationResponse<GetCollectionInTargetMarkResponse>>> GetCollectionInTargetMark(
            RequestOptionsBase<GetCollectionInTargetMarkFilterOption, GetCollectionInTargetMarkSortOption> request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);

        var paginationResponse = await _targetMarkService.GetCollectionInTargetMark(userId, request);

        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetCollectionInTargetMarkResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get collections in target mark successfully";

        return Ok(basePaginationResponse);
    }
}
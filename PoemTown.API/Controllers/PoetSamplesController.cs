using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.PoetSampleRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoetSampleResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.PoetSampleFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoetSampleSorts;

namespace PoemTown.API.Controllers;

public class PoetSamplesController : BaseController
{
    private readonly IPoetSampleService _poetSampleService;
    private readonly IMapper _mapper;
    public PoetSamplesController(IPoetSampleService poetSampleService,
        IMapper mapper)
    {
        _poetSampleService = poetSampleService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Tạo mới thông tin nhà thơ nổi tiếng, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> CreatePoetSample(CreateNewPoetSampleRequest request)
    {
        await _poetSampleService.CreateNewPoetSample(request);
        return Created(String.Empty, new BaseResponse(StatusCodes.Status201Created, "Create poet sample successfully"));
    }
    
    /// <summary>
    /// Cập nhật thông tin nhà thơ nổi tiếng, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> UpdatePoetSample(UpdatePoetSampleRequest request)
    {
        await _poetSampleService.UpdatePoetSample(request);
        return Accepted(new BaseResponse(StatusCodes.Status202Accepted, "Update poet sample successfully"));
    }
    
    /// <summary>
    /// Xóa thông tin nhà thơ nổi tiếng, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="poetSampleId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> DeletePoetSample(Guid poetSampleId)
    {
        await _poetSampleService.DeletePoetSample(poetSampleId);
        return Accepted(new BaseResponse(StatusCodes.Status202Accepted, "Delete poet sample successfully"));
    }
    
    /// <summary>
    /// Đăng tải ảnh đại diện nhà thơ nổi tiếng lên AWS S3, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/image")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> UploadPoetSampleAvatar(IFormFile file)
    {
        string imageUrl = await _poetSampleService.UploadPoetSampleAvatar(file);
        return Created(String.Empty, new BaseResponse(StatusCodes.Status201Created, "Upload poet sample avatar successfully", imageUrl));
    }
    
    /// <summary>
    /// Lấy danh sách nhà thơ nổi tiếng, không yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1")]
    public async Task<ActionResult<BasePaginationResponse<GetPoetSampleResponse>>> 
        GetPoetSamples(RequestOptionsBase<GetPoetSampleFilterOption, GetPoetSampleSortOption> request)
    {
        var paginationResponse = await _poetSampleService.GetPoetSamples(request);

        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetPoetSampleResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get poet samples successfully";
        
        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Lấy thông tin nhà thơ nổi tiếng theo id, không yêu cầu đăng nhập
    /// </summary>
    /// <param name="poetSampleId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/{poetSampleId}")]
    public async Task<ActionResult<GetPoetSampleResponse>> GetPoetSample(Guid poetSampleId)
    {
        var poetSample = await _poetSampleService.GetPoetSample(poetSampleId);
        return Ok(new BaseResponse<GetPoetSampleResponse>(StatusCodes.Status200OK, "Get poet sample successfully", poetSample));
    }
    
    /// <summary>
    /// Xóa danh hiệu mẫu của nhà thơ nổi tiếng, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="poetSampleId"></param>
    /// <param name="titleSampleIds"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{poetSampleId}/title-samples")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> RemovePoetSampleTitleSample(Guid poetSampleId, [FromBody] IList<Guid> titleSampleIds)
    {
        await _poetSampleService.RemovePoetSampleTitleSample(poetSampleId, titleSampleIds);
        return Accepted(new BaseResponse(StatusCodes.Status202Accepted, "Remove title sample from poet sample successfully"));
    }
}
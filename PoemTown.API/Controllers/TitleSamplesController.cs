using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.TitleSampleRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.TitleSampleResponses;
using PoemTown.Service.Interfaces;

namespace PoemTown.API.Controllers;

public class TitleSamplesController : BaseController
{
    private readonly ITitleSampleService _titleSampleService;

    public TitleSamplesController(ITitleSampleService titleSampleService)
    {
        _titleSampleService = titleSampleService;
    }

    /// <summary>
    /// Lấy tất cả danh hiệu mẫu của nhà thơ nổi tiếng, Không yêu cầu đăng nhập
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("v1")]
    public async Task<ActionResult<BaseResponse<IEnumerable<GetTitleSampleResponse>>>> GetTitleSamples()
    {
        var titleSamples = await _titleSampleService.GetAllTitleSamples();
        return Ok(new BaseResponse<IEnumerable<GetTitleSampleResponse>>
        {
            Data = titleSamples,
            Message = "Get Title Samples successfully",
            StatusCode = StatusCodes.Status200OK
        });
    }

    /// <summary>
    /// Lấy danh hiệu mẫu của nhà thơ nổi tiếng theo id, Không yêu cầu đăng nhập
    /// </summary>
    /// <param name="titleSampleId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/{titleSampleId}")]
    public async Task<ActionResult<BaseResponse<GetTitleSampleResponse>>> GetTitleSample(Guid titleSampleId)
    {
        var titleSample = await _titleSampleService.GetTitleSampleById(titleSampleId);
        return Ok(new BaseResponse<GetTitleSampleResponse>
        {
            Data = titleSample,
            Message = "Get Title Sample successfully",
            StatusCode = StatusCodes.Status200OK
        });
    }

    /// <summary>
    /// Tạo mới danh hiệu mẫu của nhà thơ nổi tiếng, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> CreateTitleSample([FromBody] CreateTitleSampleRequest request)
    {
        await _titleSampleService.CreateTitleSample(request);
        return Created(String.Empty, new BaseResponse<GetTitleSampleResponse>
        {
            Message = "Create Title Sample successfully",
            StatusCode = StatusCodes.Status201Created
        });
    }

    /// <summary>
    /// Cập nhật danh hiệu mẫu của nhà thơ nổi tiếng, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="titleSampleId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/{titleSampleId}")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> UpdateTitleSample(Guid titleSampleId,
        [FromBody] UpdateTitleSampleRequest request)
    {
        await _titleSampleService.UpdateTitleSample(request);
        return Accepted(new BaseResponse<GetTitleSampleResponse>
        {
            Message = "Update Title Sample successfully",
            StatusCode = StatusCodes.Status202Accepted
        });
    }

    /// <summary>
    /// Xóa danh hiệu mẫu của nhà thơ nổi tiếng, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="titleSampleId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{titleSampleId}")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> DeleteTitleSample(Guid titleSampleId)
    {
        await _titleSampleService.DeleteTitleSample(titleSampleId);
        return Ok(new BaseResponse<GetTitleSampleResponse>
        {
            Message = "Delete Title Sample successfully",
            StatusCode = StatusCodes.Status200OK
        });
    }

    /// <summary>
    /// Xóa vĩnh viễn danh hiệu mẫu của nhà thơ nổi tiếng, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="titleSampleId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{titleSampleId}/permanent")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> PermanentDeleteTitleSample(Guid titleSampleId)
    {
        await _titleSampleService.DeleteTitleSamplePermanently(titleSampleId);
        return Ok(new BaseResponse<GetTitleSampleResponse>
        {
            Message = "Permanent Delete Title Sample successfully",
            StatusCode = StatusCodes.Status200OK
        });
    }

    /// <summary>
    /// Thêm danh hiệu mẫu vào nhà thơ nổi tiếng, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="poetSampleId"></param>
    /// <param name="titleSampleIds"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/{poetSampleId}")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> AddTitleSamplesIntoPoetSample(Guid poetSampleId, [FromQuery] List<Guid> titleSampleIds)
    {
        await _titleSampleService.AddTitleSamplesIntoPoetSample(poetSampleId, titleSampleIds);
        return Ok(new BaseResponse<GetTitleSampleResponse>
        {
            Message = "Add Title Samples into Poet Sample successfully",
            StatusCode = StatusCodes.Status200OK
        });
    }
}
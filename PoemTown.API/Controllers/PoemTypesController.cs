using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.PoemTypeRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PoemTypeResponses;
using PoemTown.Service.Interfaces;

namespace PoemTown.API.Controllers;

public class PoemTypesController : BaseController
{
    private readonly IPoemTypeService _poemTypeService;
    public PoemTypesController(IPoemTypeService poemTypeService)
    {
        _poemTypeService = poemTypeService;
    }
    
    /// <summary>
    /// Lấy tất cả các thể loại thơ
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("v1")]
    public async Task<ActionResult<BaseResponse<GetPoemTypeResponse>>> GetPoemTypes()
    {
        var poemTypes = await _poemTypeService.GetAllPoemTypes();
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Poem types retrieved successfully", poemTypes));
    }
    
    /// <summary>
    /// Lấy thể loại thơ theo id
    /// </summary>
    /// <param name="poemTypeId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/{poemTypeId}")]
    public async Task<ActionResult<BaseResponse<GetPoemTypeResponse>>> GetPoemTypeById(Guid poemTypeId)
    {
        var poemType = await _poemTypeService.GetPoemTypeById(poemTypeId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Poem type retrieved successfully", poemType));
    }
    
    /// <summary>
    /// Tạo mới thể loại thơ, yêu cầu có quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> CreatePoemType([FromBody] CreatePoemTypeRequest request)
    {
        await _poemTypeService.CreatePoemType(request);
        return Created(String.Empty, new BaseResponse(StatusCodes.Status201Created, "Poem type created successfully"));
    }
    
    /// <summary>
    /// Cập nhật thể loại thơ, yêu cầu có quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> UpdatePoemType([FromBody] UpdatePoemTypeRequest request)
    {
        await _poemTypeService.UpdatePoemType(request);
        return Accepted(new BaseResponse(StatusCodes.Status202Accepted, "Poem type updated successfully"));
    }
    
    /// <summary>
    /// Xóa thể loại thơ, yêu cầu có quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="poemTypeId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{poemTypeId}")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> DeletePoemType(Guid poemTypeId)
    {
        await _poemTypeService.DeletePoemType(poemTypeId);
        return Accepted(new BaseResponse(StatusCodes.Status202Accepted, "Poem type deleted successfully"));
    }
    
    /// <summary>
    /// Xóa vĩnh viễn thể loại thơ, yêu cầu có quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="poemTypeId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/permanent/{poemTypeId}")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> DeletePoemTypePermanent(Guid poemTypeId)
    {
        await _poemTypeService.DeletePoemTypePermanent(poemTypeId);
        return Accepted(new BaseResponse(StatusCodes.Status202Accepted, "Poem type deleted permanently successfully"));
    }
}
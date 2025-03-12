using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.RoleRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.RoleResponse;
using PoemTown.Service.Interfaces;

namespace PoemTown.API.Controllers;

public class RoleController : BaseController
{
    private readonly IRoleService _roleService;
    
    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    
    /// <summary>
    /// Tạo mới role, yêu cầu quyền ADMIN
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> CreateRole([FromBody] CreateRoleRequest request)
    {
        await _roleService.CreateRole(request);
        return Created(String.Empty, new BaseResponse(StatusCodes.Status201Created, "Role created"));
    }
    
    /// <summary>
    /// Lấy danh sách role, yêu cầu quyền ADMIN
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("v1")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse<IEnumerable<GetRoleResponse>>>> GetRoles()
    {
        var roles = await _roleService.GetRoles();
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Roles retrieved", roles));
    }
    
    /// <summary>
    /// Cập nhật role, yêu cầu quyền ADMIN
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> UpdateRole([FromBody] UpdateRoleRequest request)
    {
        await _roleService.UpdateRole(request);
        return Accepted(String.Empty, new BaseResponse(StatusCodes.Status202Accepted, "Role updated"));
    }
}
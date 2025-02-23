using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.ThemeRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.ThemeResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.ThemeFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.ThemeSorts;

namespace PoemTown.API.Controllers;

public class ThemeController : BaseController
{
    private readonly IThemeService _themeService;
    private readonly IMapper _mapper;
    
    public ThemeController(IThemeService themeService, IMapper mapper)
    {
        _themeService = themeService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Tạo mới theme cho user, yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/user")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> CreateUserTheme([FromBody] CreateUserThemeRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _themeService.CreateUserTheme(userId, request);
        return Ok(new BaseResponse(StatusCodes.Status201Created, "Theme created successfully"));
    }
    
    /// <summary>
    /// Lấy danh sách themes của user, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// sortOptions:
    ///
    /// - CreatedTimeAscending = 1 (Sắp xếp theo thời gian tạo tăng dần)
    /// - CreatedTimeDescending = 2 (Sắp xếp theo thời gian tạo giảm dần) (Mặc định)
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/user")]
    [Authorize]
    public async Task<ActionResult<BasePaginationResponse<GetThemeResponse>>>
        GetUserTheme(RequestOptionsBase<GetUserThemeFilterOption, GetUserThemeSortOption> request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        var paginationResponse = await _themeService.GetUserTheme(userId, request);
        
        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetThemeResponse>>(paginationResponse); 
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Theme retrieved successfully";
        
        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Cập nhật theme của user, yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/user")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> UpdateUserTheme(UpdateUserThemeRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _themeService.UpdateUserTheme(userId, request);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Theme updated successfully"));
    }
    
    /// <summary>
    /// Xóa theme user, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// Không thể xóa theme default
    ///
    /// Nếu xóa theme đang sử dụng (isInUse == true, nhưng không phải isDefault == true do không được quyền xóa theme default) thì sẽ tự động Set theme default thành isInUse = true 
    /// </remarks>
    /// <param name="themeId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/user/{themeId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DeleteUserTheme(Guid themeId)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _themeService.DeleteUserTheme(userId, themeId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Theme deleted successfully"));
    }
    
    /// <summary>
    /// Xóa theme user vĩnh viễn, yêu cầu đăng nhập
    /// </summary>
    /// <param name="themeId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/user/{themeId}/permanent")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DeleteUserThemePermanent(Guid themeId)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _themeService.DeleteUserThemePermanent(userId, themeId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Theme deleted permanently successfully"));
    }
}
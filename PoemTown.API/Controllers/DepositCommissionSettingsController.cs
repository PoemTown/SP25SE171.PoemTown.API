using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.DepositCommissionSettingRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.DepositCommissionSettingResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.DepositCommissionSettingFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.DepositCommissionSettingSorts;
using PoemTown.Service.Services;

namespace PoemTown.API.Controllers;

public class DepositCommissionSettingsController : BaseController
{
    private readonly IDepositCommissionSettingService _depositCommissionSettingService;
    private readonly IMapper _mapper;
    public DepositCommissionSettingsController(IDepositCommissionSettingService depositCommissionSettingService, IMapper mapper)
    {
        _depositCommissionSettingService = depositCommissionSettingService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Tạo mới cài đặt hoa hồng nạp tiền, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> CreateNewDepositCommissionSetting(CreateNewDepositCommissionSettingRequest request)
    {
        await _depositCommissionSettingService.CreateNewDepositCommissionSetting(request);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Tạo mới thành công!"));
    }
    
    /// <summary>
    /// Cập nhật cài đặt hoa hồng nạp tiền, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> UpdateDepositCommissionSetting(UpdateDepositCommissionSettingRequest request)
    {
        await _depositCommissionSettingService.UpdateDepositCommissionSetting(request);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Cập nhật thành công!"));
    }
    
    /// <summary>
    /// Xóa cài đặt hoa hồng nạp tiền, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <param name="depositCommissionSettingId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{depositCommissionSettingId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> DeleteDepositCommissionSetting(Guid depositCommissionSettingId)
    {
        await _depositCommissionSettingService.DeleteDepositCommissionSetting(depositCommissionSettingId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Xóa thành công!"));
    }
    
    /// <summary>
    /// Lấy danh sách cài đặt hoa hồng nạp tiền, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BasePaginationResponse<GetDepositCommissionSettingsResponse>>> 
        GetDepositCommissionSettings(
         RequestOptionsBase<GetDepositCommissionSettingFilterOption, GetDepositCommissionSettingSortOption> request)
    {
        var paginationResponse = await _depositCommissionSettingService.GetDepositCommissionSettings(request);

        var basePaginationResponse =
            _mapper.Map<BasePaginationResponse<GetDepositCommissionSettingsResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Lấy danh sách thành công!";

        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Lấy chi tiết cài đặt hoa hồng nạp tiền, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <param name="depositCommissionSettingId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/{depositCommissionSettingId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse<GetDepositCommissionSettingsResponse>>> 
        GetDepositCommissionSettingDetail(Guid depositCommissionSettingId)
    {
        var depositCommissionSetting = await _depositCommissionSettingService.GetDepositCommissionSettingDetail(depositCommissionSettingId);
        var baseResponse = new BaseResponse<GetDepositCommissionSettingsResponse>(StatusCodes.Status200OK, "Lấy chi tiết thành công!", depositCommissionSetting);
        return Ok(baseResponse);
    }
    
    /// <summary>
    /// Lấy cài đặt hoa hồng nạp tiền đang sử dụng, Không yêu cầu đăng nhập
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/in-use")]
    public async Task<ActionResult<BaseResponse<GetDepositCommissionSettingsResponse>>> 
        GetInUseDepositCommissionSetting()
    {
        var depositCommissionSetting = await _depositCommissionSettingService.GetInUseDepositCommissionSetting();
        var baseResponse = new BaseResponse<GetDepositCommissionSettingsResponse>(StatusCodes.Status200OK, "Lấy chi tiết thành công!", depositCommissionSetting);
        return Ok(baseResponse);
    }
    
}
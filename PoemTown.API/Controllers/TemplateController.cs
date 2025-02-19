using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.TemplateRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.TemplateResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.TemplateFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.TemplateSorts;

namespace PoemTown.API.Controllers;

public class TemplateController : BaseController
{
    private readonly ITemplateService _templateService;
    private readonly IMapper _mapper;
    
    public TemplateController(ITemplateService templateService, IMapper mapper)
    {
        _templateService = templateService;
        _mapper = mapper;
    }
    
    
    /// <summary>
    /// Tạo mới một master template, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <remarks>
    /// Template Status:
    ///
    /// - Active = 1,
    /// - Inactive = 2,
    /// - OutOfStock = 3
    ///
    /// TemplateDetail Type:
    ///
    /// - Header = 1,
    /// - NavBackground = 2,
    /// - NavBorder = 3,
    /// - MainBackground = 4,
    /// - AchievementBorder = 5,
    /// - AchievementBackground = 6,
    /// - vStatisticBorder = 7,
    /// - StatisticBackground = 8,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/master-templates")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> CreateMasterTemplate(CreateMasterTemplateRequest request)
    {
        await _templateService.CreateMasterTemplate(request);
        return Ok(new BaseResponse(StatusCodes.Status201Created, "Master template created successfully"));
    }
    
    /// <summary>
    /// Lấy tât cả master template, không yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// Max page size: 250, nếu muốn lấy thêm, set allowExceedPageSize = true và sau đó có thể set pageSize lớn hơn 250
    /// 
    /// Status:
    /// 
    /// - Active = 1,
    /// - Inactive = 2,
    /// - OutOfStock = 3
    ///
    /// Type:
    ///
    /// - Bundle = 1,
    /// - Single = 2
    ///
    /// SortOptions:
    ///
    /// - CreatedTimeAscending = 1 (Sắp xếp theo thời gian tạo tăng dần),
    /// - CreatedTimeDescending = 2 (Sắp xếp theo thời gian tạo giảm dần) (Mặc định),
    /// - PriceAscending = 3 (Sắp xếp theo giá tăng dần),
    /// - PriceDescending = 4 (Sắp xếp theo giá giảm dần)
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/master-templates")]
    public async Task<ActionResult<BasePaginationResponse<GetMasterTemplateResponse>>> GetMasterTemplate
        (RequestOptionsBase<GetMasterTemplateFilterOption, GetMasterTemplateSortOption> request)
    {
        var paginationResponse = await _templateService.GetMasterTemplate(request);
        
        var basePaginationResponse =  _mapper.Map<BasePaginationResponse<GetMasterTemplateResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get master template successfully";
        
        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Lấy chi tiết (các thành phần) một master template, không yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// Max page size: 250, nếu muốn lấy thêm, set allowExceedPageSize = true và sau đó có thể set pageSize lớn hơn 250
    ///
    /// Type:
    ///
    /// - Header = 1,
    /// - NavBackground = 2,
    /// - NavBorder = 3,
    /// - MainBackground = 4,
    /// - AchievementBorder = 5,
    /// - AchievementBackground = 6,
    /// - StatisticBorder = 7,
    /// - StatisticBackground = 8,
    ///
    /// DesignType:
    ///
    /// - 1: Image,
    /// - 2: ColorCode
    /// 
    /// SortOptions:
    ///
    /// - CreatedTimeAscending = 1 (Sắp xếp theo thời gian tạo tăng dần),
    /// - CreatedTimeDescending = 2 (Sắp xếp theo thời gian tạo giảm dần) (Mặc định),
    /// </remarks>
    /// <param name="masterTemplateId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/master-templates/{masterTemplateId}")]
    public async Task<ActionResult<BasePaginationResponse<GetMasterTemplateDetailResponse>>>
        GetMasterTemplateDetail(Guid masterTemplateId,
            RequestOptionsBase<GetMasterTemplateDetailFilterOption, GetMasterTemplateDetailSortOption> request)
    {
        var paginationResponse = await _templateService.GetMasterTemplateDetail(masterTemplateId, request);
        
        var basePaginationResponse =  _mapper.Map<BasePaginationResponse<GetMasterTemplateDetailResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get master template detail successfully";
        
        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Cập nhật một master template, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <remarks>
    /// Status:
    /// 
    /// - Active = 1,
    /// - Inactive = 2,
    /// - OutOfStock = 3
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/master-templates")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> UpdateMasterTemplate(UpdateMasterTemplateRequest request)
    {
        await _templateService.UpdateMasterTemplate(request);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Master template updated successfully"));
    }
    
    /// <summary>
    /// Cập nhật một master template detail, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <remarks>
    /// - Chỉ được cập nhật property Image khi DesignType = 1
    ///
    /// - Chỉ được cập nhật property ColorCode khi DesignType = 2
    ///
    /// DesignType:
    ///
    /// - 1: Image,
    /// - 2: ColorCode
    ///
    /// Type:
    ///
    /// - Header = 1,
    /// - NavBackground = 2,
    /// - NavBorder = 3,
    /// - MainBackground = 4,
    /// - AchievementBorder = 5,
    /// - AchievementBackground = 6,
    /// - StatisticBorder = 7,
    /// - StatisticBackground = 8,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/master-templates/detail")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> UpdateMasterTemplateDetail(UpdateMasterTemplateDetailRequest request)
    {
        await _templateService.UpdateMasterTemplateDetail(request);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Master template detail updated successfully"));
    }
    
    /// <summary>
    /// Xóa một master template, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <param name="masterTemplateId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/master-templates/{masterTemplateId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> DeleteMasterTemplate(Guid masterTemplateId)
    {
        await _templateService.DeleteMasterTemplate(masterTemplateId);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Master template deleted successfully"));
    }
    
    /// <summary>
    /// Xóa một master template vĩnh viễn, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <param name="masterTemplateId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/master-templates/{masterTemplateId}/permanently")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> DeleteMasterTemplatePermanently(Guid masterTemplateId)
    {
        await _templateService.DeleteMasterTemplatePermanently(masterTemplateId);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Master template deleted permanently successfully"));
    }
    
    /// <summary>
    /// Xóa một master template detail, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <param name="masterTemplateDetailId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/master-templates/detail/{masterTemplateDetailId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> DeleteMasterTemplateDetail(Guid masterTemplateDetailId)
    {
        await _templateService.DeleteMasterTemplateDetail(masterTemplateDetailId);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Master template detail deleted successfully"));
    }
    
    /// <summary>
    /// Xóa một master template detail vĩnh viễn, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <param name="masterTemplateDetailId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/master-templates/detail/{masterTemplateDetailId}/permanently")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> DeleteMasterTemplateDetailPermanently(Guid masterTemplateDetailId)
    {
        await _templateService.DeleteMasterTemplateDetailPermanently(masterTemplateDetailId);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Master template detail deleted permanently successfully"));
    }
}
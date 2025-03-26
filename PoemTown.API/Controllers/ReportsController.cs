using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.ReportRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.ReportResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.ReportFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.ReportSorts;

namespace PoemTown.API.Controllers;

public class ReportsController : BaseController
{
    private readonly IReportService _reportService;
    private readonly IMapper _mapper;
    public ReportsController(IReportService reportService, IMapper mapper)
    {
        _reportService = reportService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Report bài thơ, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// USER KHÔNG THỂ REPORT BÀI THƠ CỦA CHÍNH MÌNH
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/poem")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> CreateReportPoem([FromBody] CreateReportPoemRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _reportService.CreateReportPoem(userId, request);
        return Ok(new BaseResponse(StatusCodes.Status201Created, "Report created successfully"));
    }
    
    /// <summary>
    /// Lấy danh sách các report, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <remarks>
    /// reportStatus:
    ///
    /// - Pending = 1,
    /// - Approved = 2,
    /// - Rejected = 3
    ///
    /// sortOptions:
    /// 
    /// - CreatedTimeAscending = 1,
    /// - CreatedTimeDescending = 2
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/reports")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BasePaginationResponse<GetReportResponse>>> 
        GetReports([FromQuery] RequestOptionsBase<GetReportFilterOption, GetReportSortOption> request)
    {
        var paginationResponse = await _reportService.GetReports(request);

        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetReportResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Reports retrieved successfully";
        
        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Lấy danh sách các report của bản thân, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// reportStatus:
    ///
    /// - Pending = 1,
    /// - Approved = 2,
    /// - Rejected = 3
    ///
    /// sortOptions:
    /// 
    /// - CreatedTimeAscending = 1,
    /// - CreatedTimeDescending = 2
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/mine")]
    [Authorize]
    public async Task<ActionResult<BasePaginationResponse<GetMyReportResponse>>> 
        GetMyReports([FromQuery] RequestOptionsBase<GetMyReportFilterOption, GetMyReportSortOption> request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        var paginationResponse = await _reportService.GetMyReports(userId, request);

        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetMyReportResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Reports retrieved successfully";
        
        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Xử lý report, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <remarks>
    /// reportStatus:
    ///
    /// - Pending = 1,
    /// - Approved = 2,
    /// - Rejected = 3
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/resolve")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> ResolveReport([FromBody] ResolveReportRequest request)
    {
        await _reportService.ResolveReport(request);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Report resolved successfully"));
    }
    
    [HttpPost]
    [Route("v1/user")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> CreateReportUser([FromBody] CreateReportUserRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _reportService.CreateReportUser(userId, request);
        return Ok(new BaseResponse(StatusCodes.Status201Created, "Report created successfully"));
    }
}
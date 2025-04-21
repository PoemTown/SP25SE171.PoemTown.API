using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Repository.Enums.Reports;
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
    /// type:
    ///
    /// - Poem = 1,
    /// - User = 2,
    /// - Plagiarism = 3
    /// 
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
    /// type:
    ///
    /// - Poem = 1,
    /// - User = 2,
    /// - Plagiarism = 3
    /// 
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
    
    /// <summary>
    /// Report người dùng, yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/user")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> CreateReportUser([FromBody] CreateReportUserRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _reportService.CreateReportUser(userId, request);
        return Ok(new BaseResponse(StatusCodes.Status201Created, "Report created successfully"));
    }
    
    /// <summary>
    /// Report message, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <remarks>
    /// type:
    ///
    /// - Poem = 1,
    /// - User = 2,
    /// - Plagiarism = 3
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/message")]
    [Authorize(Roles = "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> CreateReportMessage([FromBody] CreateReportMessageRequest request)
    {
        await _reportService.CreateReportMessage(request);
        return Created(string.Empty, new BaseResponse(StatusCodes.Status201Created, "Report created successfully"));
    }
    
    /// <summary>
    /// Cập nhật report message, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <remarks>
    /// type:
    ///
    /// - Poem = 1,
    /// - User = 2,
    /// - Plagiarism = 3
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/message")]
    [Authorize(Roles= "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> UpdateReportMessage([FromBody] UpdateReportMessageRequest request)
    {
        await _reportService.UpdateReportMessage(request);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Report resolved successfully"));
    }
    
    /// <summary>
    /// Xóa report message, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="reportMessageId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/message/{reportMessageId}")]
    [Authorize(Roles= "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> DeleteReportMessage(Guid reportMessageId)
    {
        await _reportService.DeleteReportMessage(reportMessageId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Report deleted successfully"));
    }
    
    /// <summary>
    /// Xóa report message vĩnh viễn, yêu cầu đăng nhập dưới quyền ADMIN hoặc MODERATOR
    /// </summary>
    /// <param name="reportMessageId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/message/{reportMessageId}/permanent")]
    [Authorize(Roles= "ADMIN, MODERATOR")]
    public async Task<ActionResult<BaseResponse>> DeleteReportMessagePermanent(Guid reportMessageId)
    {
        await _reportService.DeleteReportMessagePermanent(reportMessageId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Report deleted successfully"));
    }
    
    /// <summary>
    /// Lấy thông tin một report message theo id, không yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// type:
    ///
    /// - Poem = 1,
    /// - User = 2,
    /// - Plagiarism = 3
    /// </remarks>
    /// <param name="reportMessageId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/message/{reportMessageId}")]
    public async Task<ActionResult<BaseResponse<GetReportMessageResponse>>> GetReportMessage(Guid reportMessageId)
    {
        var response = await _reportService.GetReportMessage(reportMessageId);
        return Ok(new BaseResponse<GetReportMessageResponse>(StatusCodes.Status200OK, "Report retrieved successfully", response));
    }
    
    /// <summary>
    /// Lấy danh sách các report message, không yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// type:
    ///
    /// - Poem = 1,
    /// - User = 2,
    /// - Plagiarism = 3
    /// </remarks>
    /// <param name="type"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/messages")]
    public async Task<ActionResult<BaseResponse<IList<GetReportMessageResponse>>>> GetReportMessages([FromQuery] ReportType? type)
    {
        var response = await _reportService.GetReportMessages(type);
        return Ok(new BaseResponse<IList<GetReportMessageResponse>>(StatusCodes.Status200OK, "Reports retrieved successfully", response));
    }
}
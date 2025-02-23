using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.ReportRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.Interfaces;

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
}
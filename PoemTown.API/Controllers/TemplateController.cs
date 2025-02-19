using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.TemplateRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.Interfaces;

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
}
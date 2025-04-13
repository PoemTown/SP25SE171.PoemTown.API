using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.WithdrawalFormRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.WithdrawalFormResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.WithdrawalFormFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.WithdrawalFormSorts;

namespace PoemTown.API.Controllers;

public class WithdrawalFormsController : BaseController
{
    private readonly IWithdrawalFormService _withdrawalFormService;
    private readonly IMapper _mapper;

    public WithdrawalFormsController(IWithdrawalFormService withdrawalFormService, IMapper mapper)
    {
        _withdrawalFormService = withdrawalFormService;
        _mapper = mapper;
    }

    /// <summary>
    /// Lấy tất cả các đơn rút tiền từ người dùng, yêu cầu đăng nhập với quyền ADMIN
    /// </summary>
    /// <remarks>
    /// status:
    ///
    /// - Accepted = 1,
    /// - Rejected = 2,
    /// - Pending = 3,
    ///
    /// bankType:
    ///
    /// - Momo = 1,
    /// - TpBank = 2,
    ///
    /// sortOptions:
    ///
    /// - CreatedTimeAscending = 1,
    /// - CreatedTimeDescending = 2,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BasePaginationResponse<GetWithdrawalFormResponse>>> GetWithdrawalForms(
        RequestOptionsBase<GetWithdrawalFormFilterOption, GetWithdrawalFormSortOption> request)
    {
        var paginationResponse = await _withdrawalFormService.GetWithdrawalForms(request);
        
        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetWithdrawalFormResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get withdrawal forms successfully";
        
        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Người dùng lấy tất cả các đơn rút tiền của bản thân, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// status:
    ///
    /// - Accepted = 1,
    /// - Rejected = 2,
    /// - Pending = 3,
    ///
    /// bankType:
    ///
    /// - Momo = 1,
    /// - TpBank = 2,
    ///
    /// sortOptions:
    ///
    /// - CreatedTimeAscending = 1,
    /// - CreatedTimeDescending = 2,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/mine")]
    [Authorize]
    public async Task<ActionResult<BasePaginationResponse<GetWithdrawalFormResponse>>> GetMyWithdrawalForms(
        RequestOptionsBase<GetWithdrawalFormFilterOption, GetWithdrawalFormSortOption> request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value);
        
        var paginationResponse = await _withdrawalFormService.GetMyWithdrawalForms(userId, request);
        
        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetWithdrawalFormResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get my withdrawal forms successfully";
        
        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Lấy thông tin chi tiết đơn rút tiền, yêu cầu đăng nhập
    /// </summary>
    /// <param name="withdrawalFormId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/{withdrawalFormId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<GetWithdrawalFormResponse>>> GetWithdrawalFormDetail(Guid withdrawalFormId)
    {
        var response = await _withdrawalFormService.GetWithdrawalFormDetail(withdrawalFormId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Get withdrawal form detail successfully", response));
    }
    
    /// <summary>
    /// Xử lý đơn rút tiền, yêu cầu đăng nhập với quyền ADMIN
    /// </summary>
    /// <remarks>
    /// status:
    ///
    /// - Accepted = 1,
    /// - Rejected = 2,
    /// - Pending = 3,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/resolve")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> ResolveWithdrawalForm(ResolveWithdrawalFormRequest request)
    {
        await _withdrawalFormService.ResolveWithdrawalForm(request);
        return Accepted(new BaseResponse(StatusCodes.Status202Accepted, "Resolve withdrawal form successfully"));
    }
    
    /// <summary>
    /// Tải lên bằng chứng xử lý đơn rút tiền, yêu cầu đăng nhập với quyền ADMIN
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/upload-evidence")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse<string>>> UploadWithdrawalFormEvidence(IFormFile file)
    {
        var evidenceUrl = await _withdrawalFormService.UploadWithdrawalFormEvidence(file);
        return Created(String.Empty, new BaseResponse(StatusCodes.Status201Created, "Upload evidence successfully", evidenceUrl));
    }
}
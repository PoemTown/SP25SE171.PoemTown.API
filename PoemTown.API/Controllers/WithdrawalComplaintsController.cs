using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.WithdrawalComplaintRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.WithdrawalComplaintResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.WithdrawalComplaintFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.WithdrawalComplaintSorts;

namespace PoemTown.API.Controllers;

public class WithdrawalComplaintsController : BaseController
{
    private readonly IWithdrawalComplaintService _withdrawalComplaintService;
    private readonly IMapper _mapper;

    public WithdrawalComplaintsController(IWithdrawalComplaintService withdrawalComplaintService, IMapper mapper)
    {
        _withdrawalComplaintService = withdrawalComplaintService;
        _mapper = mapper;
    }

    /// <summary>
    /// Tạo mới khiếu nại rút tiền, yêu cầu đăng nhập
    /// </summary>
    /// <param name="withdrawalFormId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/{withdrawalFormId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> CreateNewWithdrawalComplaint(Guid withdrawalFormId,
        CreateNewWithdrawalComplaintRequest request)
    {
        await _withdrawalComplaintService.CreateNewWithdrawalComplaint(withdrawalFormId, request);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Tạo mới thành công!"));
    }

    /// <summary>
    /// Cập nhật khiếu nại rút tiền, yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> UpdateWithdrawalComplaint(UpdateWithdrawalComplaintRequest request)
    {
        await _withdrawalComplaintService.UpdateWithdrawalComplaint(request);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Cập nhật thành công!"));
    }

    /// <summary>
    /// Xóa khiếu nại rút tiền, yêu cầu đăng nhập
    /// </summary>
    /// <param name="withdrawalComplaintId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{withdrawalComplaintId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DeleteWithdrawalComplaint(Guid withdrawalComplaintId)
    {
        await _withdrawalComplaintService.DeleteWithdrawalComplaint(withdrawalComplaintId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Xóa thành công!"));
    }

    /// <summary>
    /// Tải ảnh khiếu nại rút tiền, yêu cầu đăng nhập
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/image")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> UploadWithdrawalComplaintImage(IFormFile file)
    {
        var image = await _withdrawalComplaintService.UploadWithdrawalComplaintImage(file);
        return Ok(new BaseResponse<string>(StatusCodes.Status200OK, "Tải ảnh thành công!", image));
    }

    /// <summary>
    /// Thêm ảnh vào khiếu nại rút tiền, yêu cầu đăng nhập
    /// </summary>
    /// <param name="withdrawalComplaintId"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/{withdrawalComplaintId}/image")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> AddImageToUpdateWithdrawalComplaint(Guid withdrawalComplaintId,
        string image)
    {
        await _withdrawalComplaintService.AddImageToUpdateWithdrawalComplaint(withdrawalComplaintId, image);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Thêm ảnh thành công!"));
    }

    /// <summary>
    /// Xóa ảnh khỏi khiếu nại rút tiền, yêu cầu đăng nhập
    /// </summary>
    /// <param name="withdrawalComplaintId"></param>
    /// <param name="withdrawalComplaintImageId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{withdrawalComplaintId}/image")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DeleteImageFromWithdrawalComplaint(Guid withdrawalComplaintId,
        Guid withdrawalComplaintImageId)
    {
        await _withdrawalComplaintService.DeleteImageFromWithdrawalComplaint(withdrawalComplaintId,
            withdrawalComplaintImageId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Xóa ảnh thành công!"));
    }

    /// <summary>
    /// Lấy danh sách khiếu nại rút tiền, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <remarks>
    /// status:
    ///
    /// - Accepted = 1,
    /// - Rejected = 2,
    /// - Pending = 3,
    ///
    /// image.type:
    ///
    /// - Complaint = 1,
    /// - ResolveComplaint = 2,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BasePaginationResponse<GetWithdrawalComplaintResponse>>> GetWithdrawalComplaints(
        RequestOptionsBase<GetWithdrawalComplaintFilterOption, GetWithdrawalComplaintSortOption> request)
    {
        var paginationResponse = await _withdrawalComplaintService.GetWithdrawalComplaints(request);

        var basePaginationResponse =
            _mapper.Map<BasePaginationResponse<GetWithdrawalComplaintResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Lấy danh sách khiếu nại thành công!";
        return Ok(basePaginationResponse);
    }

    /// <summary>
    /// Lấy danh sách khiếu nại rút tiền của tôi, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// status:
    ///
    /// - Accepted = 1,
    /// - Rejected = 2,
    /// - Pending = 3,
    ///
    /// image.type:
    ///
    /// - Complaint = 1,
    /// - ResolveComplaint = 2,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/mine")]
    [Authorize]
    public async Task<ActionResult<BasePaginationResponse<GetWithdrawalComplaintResponse>>> GetMyWithdrawalComplaints(
        RequestOptionsBase<GetWithdrawalComplaintFilterOption, GetWithdrawalComplaintSortOption> request)
    {
        var userId = Guid.Parse(User.FindFirst("UserId")?.Value ?? string.Empty);
        var paginationResponse = await _withdrawalComplaintService.GetMyWithdrawalComplaints(userId, request);

        var basePaginationResponse =
            _mapper.Map<BasePaginationResponse<GetWithdrawalComplaintResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Lấy danh sách khiếu nại của tôi thành công!";
        return Ok(basePaginationResponse);
    }

    /// <summary>
    /// Lấy chi tiết khiếu nại rút tiền, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// status:
    ///
    /// - Accepted = 1,
    /// - Rejected = 2,
    /// - Pending = 3,
    ///
    /// image.type:
    ///
    /// - Complaint = 1,
    /// - ResolveComplaint = 2,
    /// </remarks>
    /// <param name="withdrawalComplaintId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/{withdrawalComplaintId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<GetWithdrawalComplaintResponse>>> GetWithdrawalComplaintById(
        Guid withdrawalComplaintId)
    {
        var withdrawalComplaint = await _withdrawalComplaintService.GetWithdrawalComplaintById(withdrawalComplaintId);

        return Ok(new BaseResponse(StatusCodes.Status200OK, "Lấy chi tiết khiếu nại thành công!", withdrawalComplaint));
    }

    /// <summary>
    /// Giải quyết khiếu nại rút tiền, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <remarks>
    /// status:
    ///
    /// - Accepted = 1,
    /// - Rejected = 2,
    /// - Pending = 3,
    /// </remarks>
    /// <param name="withdrawalComplaintId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/{withdrawalComplaintId}/resolve")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> ResolveWithdrawalComplaint(Guid withdrawalComplaintId,
        ResolveWithdrawalComplaintRequest request)
    {
        await _withdrawalComplaintService.ResolveWithdrawalComplaint(withdrawalComplaintId, request);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Giải quyết khiếu nại thành công!"));
    }
}
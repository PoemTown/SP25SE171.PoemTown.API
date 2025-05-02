using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.BankTypeRequests;
using PoemTown.Service.BusinessModels.ResponseModels.BankTypeResponses;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.BankTypeFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.BankTypeSorts;

namespace PoemTown.API.Controllers;

public class BankTypesController : BaseController
{
    private readonly IBankTypeService _bankTypeService;
    private readonly IMapper _mapper;
    public BankTypesController(IBankTypeService bankTypeService, IMapper mapper)
    {
        _bankTypeService = bankTypeService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Tạo mới ngân hàng, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <remarks>
    /// status:
    /// 
    /// - Active = 1,
    /// - Inactive = 2,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> CreateNewBankType(CreateNewBankTypeRequest request)
    {
        await _bankTypeService.CreateNewBankType(request);
        return Created(String.Empty, new BaseResponse(StatusCodes.Status201Created, "Tạo mới thành công!"));
    }
    
    /// <summary>
    /// Cập nhật ngân hàng, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <remarks>
    /// status:
    /// 
    /// - Active = 1,
    /// - Inactive = 2,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> UpdateBankType(UpdateBankTypeRequest request)
    {
        await _bankTypeService.UpdateBankType(request);
        return Accepted(new BaseResponse(StatusCodes.Status202Accepted, "Cập nhật thành công!"));
    }
    
    /// <summary>
    /// Xóa ngân hàng, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <param name="bankTypeId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{bankTypeId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> DeleteBankType(Guid bankTypeId)
    {
        await _bankTypeService.DeleteBankType(bankTypeId);
        return Accepted(new BaseResponse(StatusCodes.Status202Accepted, "Xóa thành công!"));
    }
    
    /// <summary>
    /// Lấy chi tiết ngân hàng, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// status:
    /// 
    /// - Active = 1,
    /// - Inactive = 2,
    /// </remarks>
    /// <param name="bankTypeId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/{bankTypeId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<GetBankTypeResponse>>> GetBankTypeDetail(Guid bankTypeId)
    {
        var result = await _bankTypeService.GetBankTypeDetail(bankTypeId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Lấy chi tiết ngân hàng thành công!", result));
    }
    
    /// <summary>
    /// Lấy danh sách ngân hàng, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <remarks>
    /// status:
    /// 
    /// - Active = 1,
    /// - Inactive = 2,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BasePaginationResponse<GetBankTypeResponse>>> GetBankTypes(
        RequestOptionsBase<GetBankTypeFilterOption, GetBankTypeSortOption> request)
    {
        var paginationResponse = await _bankTypeService.GetBankTypes(request);

        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetBankTypeResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Lấy danh sách ngân hàng thành công!";
        
        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Tải lên icon ngân hàng, yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/upload-icon")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> UploadBankTypeImageIcon(IFormFile file)
    {
        var imageUrl = await _bankTypeService.UploadBankTypeImageIcon(file);
        return Created(String.Empty, new BaseResponse<string>(StatusCodes.Status201Created, "Upload icon thành công!", imageUrl));
    }
    
    /// <summary>
    /// Người dùng lấy danh sách ngân hàng, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// status:
    /// 
    /// - Active = 1,
    /// - Inactive = 2,
    /// </remarks>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/user")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<GetBankTypeResponse>>> UserGetBankTypes(
        GetBankTypeFilterOption filter)
    {
        var result = await _bankTypeService.UserGetBankTypes(filter);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Lấy danh sách ngân hàng thành công!", result));
    }
    
    /// <summary>
    /// Tạo mới tài khoản ngân hàng người dùng, yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/user-bank-types")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> CreateUserBankType(CreateUserBankTypeRequest request)
    {
        var userId = Guid.Parse(User.FindFirst("UserId")?.Value ?? string.Empty);

        await _bankTypeService.CreateUserBankType(userId, request);
        return Created(String.Empty, new BaseResponse(StatusCodes.Status201Created, "Tạo mới tài khoản ngân hàng thành công!"));
    }
    
    /// <summary>
    /// Cập nhật tài khoản ngân hàng người dùng, yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/user-bank-types")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> UpdateUserBankType(UpdateUserBankTypeRequest request)
    {
        var userId = Guid.Parse(User.FindFirst("UserId")?.Value ?? string.Empty);

        await _bankTypeService.UpdateUserBankType(userId, request);
        return Accepted(new BaseResponse(StatusCodes.Status202Accepted, "Cập nhật tài khoản ngân hàng thành công!"));
    }
    
    /// <summary>
    /// Xóa tài khoản ngân hàng người dùng, yêu cầu đăng nhập
    /// </summary>
    /// <param name="userBankTypeId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/user-bank-types/{userBankTypeId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DeleteUserBankType(Guid userBankTypeId)
    {
        await _bankTypeService.DeleteUserBankType(userBankTypeId);
        return Accepted(new BaseResponse(StatusCodes.Status202Accepted, "Xóa tài khoản ngân hàng thành công!"));
    }
    
    /// <summary>
    /// Lấy chi tiết tài khoản ngân hàng người dùng, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// status:
    /// 
    /// - Active = 1,
    /// - Inactive = 2,
    /// </remarks>
    /// <param name="userBankTypeId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/user-bank-types/{userBankTypeId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<GetUserBankTypeResponse>>> GetUserBankTypeDetail(Guid userBankTypeId)
    {
        var result = await _bankTypeService.GetUserBankTypeDetail(userBankTypeId);
        return Ok(new BaseResponse(StatusCodes.Status200OK, "Lấy chi tiết tài khoản ngân hàng thành công!", result));
    }

    /// <summary>
    /// Lấy danh sách tài khoản ngân hàng người dùng, yêu cầu đăng nhậpz
    /// </summary>
    /// <remarks>
    /// status:
    /// 
    /// - Active = 1,
    /// - Inactive = 2,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/user-bank-types")]
    [Authorize]
    public async Task<ActionResult<BasePaginationResponse<GetUserBankTypeResponse>>> 
        GetUserBankTypes(RequestOptionsBase<GetUserBankTypeFilterOption, GetUserBankTypeSortOption> request)
    {
        var userId = Guid.Parse(User.FindFirst("UserId")?.Value ?? string.Empty);
        
        var paginationResponse = await _bankTypeService.GetUserBankTypes(userId, request);

        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetUserBankTypeResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Lấy danh sách tài khoản ngân hàng thành công!";
        
        return Ok(basePaginationResponse);
    }
}
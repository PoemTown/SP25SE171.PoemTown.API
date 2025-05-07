using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.TransactionResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.TransactionFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.TransactionSorts;

namespace PoemTown.API.Controllers;

public class TransactionsController : BaseController
{
    private readonly ITransactionService _transactionService;
    private readonly IMapper _mapper;
    public TransactionsController(ITransactionService transactionService,
        IMapper mapper)
    {
        _transactionService = transactionService;
        _mapper = mapper;
    }

    /// <summary>
    /// Người dùng lấy lịch sử giao dịch của mình, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// TransactionType:
    ///
    /// - EWalletDeposit = 1,
    /// - MasterTemplates = 2,
    /// - RecordFiles = 3,
    /// - Poems = 4,
    /// - Withdraw = 5,
    /// - Donate = 6,
    /// - CommissionFee = 7,
    /// - Refund = 8,
    /// 
    /// SortOptions:
    ///
    /// - CreatedTimeAscending = 1,
    /// - CreatedTimeDescending = 2,
    /// - AmountAscending = 3,
    /// - AmountDescending = 4,
    ///
    /// status:
    ///
    /// - Pending = 1,
    /// - Paid = 2,
    /// - Cancelled = 3,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/mine")]
    [Authorize]
    public async Task<BasePaginationResponse<UserGetTransactionResponse>> GetMyTransaction
        (RequestOptionsBase<UserGetTransactionFilterOption, UserGetTransactionSortOption> request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);

        var paginationResponse = await _transactionService.UserGetTransactions(userId, request);

        var basePaginationResponse =
            _mapper.Map<BasePaginationResponse<UserGetTransactionResponse>>(paginationResponse);

        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get my transactions successfully";
        
        return basePaginationResponse;
    }
    
    /// <summary>
    /// Admin lấy lịch sử giao dịch của tất cả người dùng, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// TransactionType:
    ///
    /// - EWalletDeposit = 1,
    /// - MasterTemplates = 2,
    /// - RecordFiles = 3,
    /// - Poems = 4,
    /// - Withdraw = 5,
    /// - Donate = 6,
    /// - CommissionFee = 7,
    /// - Refund = 8,
    /// 
    /// SortOptions:
    ///
    /// - CreatedTimeAscending = 1,
    /// - CreatedTimeDescending = 2,
    /// - AmountAscending = 3,
    /// - AmountDescending = 4,
    /// - TransactionTypeAscending = 5,
    /// - TransactionTypeDescending = 6,
    ///
    /// status:
    ///
    /// - Pending = 1,
    /// - Paid = 2,
    /// - Cancelled = 3,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/admin")]
    [Authorize(Roles = "ADMIN")]
    public async Task<BasePaginationResponse<GetTransactionResponse>> GetTransactions
        (RequestOptionsBase<GetTransactionFilterOption, GetTransactionSortOption> request)
    {
        var paginationResponse = await _transactionService.GetTransactions(request);

        var basePaginationResponse =
            _mapper.Map<BasePaginationResponse<GetTransactionResponse>>(paginationResponse);

        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get transactions successfully";
        
        return basePaginationResponse;
    }
    
    /// <summary>
    /// Admin lấy chi tiết giao dịch, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// TransactionType:
    ///
    /// - EWalletDeposit = 1,
    /// - MasterTemplates = 2,
    /// - RecordFiles = 3,
    /// - Poems = 4,
    /// - Withdraw = 5,
    /// - Donate = 6,
    /// - CommissionFee = 7,
    /// - Refund = 8,
    ///
    /// status:
    ///
    /// - Pending = 1,
    /// - Paid = 2,
    /// - Cancelled = 3,
    /// </remarks>
    /// <param name="transactionId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/admin/{transactionId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<BaseResponse<GetTransactionDetailResponse>> GetTransactionDetail(Guid transactionId)
    {
        var transactionDetail = await _transactionService.GetTransactionDetail(transactionId);
        
        return new BaseResponse<GetTransactionDetailResponse>(StatusCodes.Status200OK, "Get transaction detail successfully", transactionDetail);
    }
    
    /// <summary>
    /// Người dùng lấy chi tiết giao dịch, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// TransactionType:
    ///
    /// - EWalletDeposit = 1,
    /// - MasterTemplates = 2,
    /// - RecordFiles = 3,
    /// - Poems = 4,
    /// - Withdraw = 5,
    /// - Donate = 6,
    /// - CommissionFee = 7,
    /// - Refund = 8,
    ///
    /// status:
    ///
    /// - Pending = 1,
    /// - Paid = 2,
    /// - Cancelled = 3,
    /// </remarks>
    /// <param name="transactionId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/user/{transactionId}")]
    [Authorize]
    public async Task<BaseResponse<UserGetTransactionDetailResponse>> UserGetTransactionDetail(Guid transactionId)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);

        var transactionDetail = await _transactionService.UserGetTransactionDetail(userId, transactionId);
        
        return new BaseResponse<UserGetTransactionDetailResponse>(StatusCodes.Status200OK, "Lấy chi tiết giao dịch thành công", transactionDetail);
    }
}
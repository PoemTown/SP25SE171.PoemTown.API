using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
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
    ///
    /// SortOptions:
    ///
    /// - CreatedTimeAscending = 1,
    /// - CreatedTimeDescending = 2,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/mine")]
    public async Task<BasePaginationResponse<UserGetTransactionResponse>> GetMyTransaction
        (RequestOptionsBase<GetTransactionFilterOption, GetTransactionSortOption> request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);

        var paginationResponse = await _transactionService.UserGetTransactions(userId, request);

        var basePaginationResponse =
            _mapper.Map<BasePaginationResponse<UserGetTransactionResponse>>(paginationResponse);

        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get my transactions successfully";
        
        return basePaginationResponse;
    }
}
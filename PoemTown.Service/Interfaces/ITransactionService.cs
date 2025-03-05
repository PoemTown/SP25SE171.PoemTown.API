using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.ResponseModels.TransactionResponses;
using PoemTown.Service.QueryOptions.FilterOptions.TransactionFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.TransactionSorts;

namespace PoemTown.Service.Interfaces;

public interface ITransactionService
{
    Task<PaginationResponse<UserGetTransactionResponse>> UserGetTransactions(Guid userId,
        RequestOptionsBase<GetTransactionFilterOption, GetTransactionSortOption> request);
}
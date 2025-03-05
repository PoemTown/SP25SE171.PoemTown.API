using AutoMapper;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.ResponseModels.TransactionResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.TransactionFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.TransactionSorts;

namespace PoemTown.Service.Services;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<PaginationResponse<UserGetTransactionResponse>> UserGetTransactions(Guid userId, 
        RequestOptionsBase<GetTransactionFilterOption, GetTransactionSortOption> request)
    {
        UserEWallet? userEWallet = await _unitOfWork.GetRepository<UserEWallet>().FindAsync(p => p.UserId == userId);
        
        // Check if user e-wallet is null
        if (userEWallet == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User EWallet not found");
        }
        
        var transactionQuery = _unitOfWork.GetRepository<Transaction>().AsQueryable();

        transactionQuery = transactionQuery.Where(p => p.UserEWalletId == userEWallet.Id);
        
        // Filter
        if (request.FilterOptions != null)
        {
            if (request.FilterOptions.Type != null)
            {
                transactionQuery = transactionQuery.Where(p => p.Type == request.FilterOptions.Type);
            }
        }
        
        // Sort
        transactionQuery = request.SortOptions switch
        {
            GetTransactionSortOption.CreatedTimeAscending => transactionQuery.OrderBy(p => p.CreatedTime),
            GetTransactionSortOption.CreatedTimeDescending => transactionQuery.OrderByDescending(p => p.CreatedTime),
            _ => transactionQuery.OrderBy(p => p.Type).ThenByDescending(p => p.CreatedTime)
        };
        
        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<Transaction>()
            .GetPagination(transactionQuery, request.PageNumber, request.PageSize);
        
        var transactions = _mapper.Map<IList<UserGetTransactionResponse>>(queryPaging.Data);

        return new PaginationResponse<UserGetTransactionResponse>(transactions, queryPaging.PageNumber,
            queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }
}
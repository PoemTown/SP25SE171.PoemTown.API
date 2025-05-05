using AutoMapper;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Transactions;
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
        RequestOptionsBase<UserGetTransactionFilterOption, UserGetTransactionSortOption> request)
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
            UserGetTransactionSortOption.CreatedTimeAscending => transactionQuery.OrderBy(p => p.CreatedTime),
            UserGetTransactionSortOption.CreatedTimeDescending =>
                transactionQuery.OrderByDescending(p => p.CreatedTime),
            UserGetTransactionSortOption.AmountAscending => transactionQuery.OrderBy(p => p.Amount),
            UserGetTransactionSortOption.AmountDescending => transactionQuery.OrderByDescending(p => p.Amount),
            _ => transactionQuery.OrderByDescending(p => p.CreatedTime)
        };

        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<Transaction>()
            .GetPagination(transactionQuery, request.PageNumber, request.PageSize);

        var transactions = _mapper.Map<IList<UserGetTransactionResponse>>(queryPaging.Data);

        return new PaginationResponse<UserGetTransactionResponse>(transactions, queryPaging.PageNumber,
            queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }


    public async Task<PaginationResponse<GetTransactionResponse>>
        GetTransactions(RequestOptionsBase<GetTransactionFilterOption, GetTransactionSortOption> request)
    {
        var adminRole = await _unitOfWork.GetRepository<Role>().FindAsync(p => p.Name == "ADMIN");
        
        // Check if admin role is null
        if (adminRole == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Admin role not found");
        }
        
        var adminUser = await _unitOfWork.GetRepository<User>().FindAsync(p => p.UserRoles.Any(p => p.RoleId == adminRole.Id));
        
        // Check if admin user is null
        if (adminUser == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Admin user not found");
        }
        
        // Check if admin user has e-wallet
        var adminUserEWallet = await _unitOfWork.GetRepository<UserEWallet>().FindAsync(p => p.UserId == adminUser.Id);
        if (adminUserEWallet == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Admin user e-wallet not found");
        }
        
        var transactionQuery = _unitOfWork.GetRepository<Transaction>()
            .AsQueryable()
            .Where(p => p.UserEWalletId != adminUserEWallet.Id);

        // Filter
        if (request.FilterOptions != null)
        {
            if (request.FilterOptions.Type != null)
            {
                transactionQuery = transactionQuery.Where(p => p.Type == request.FilterOptions.Type);
            }

            if (!string.IsNullOrWhiteSpace(request.FilterOptions.PhoneNumber))
            {
                transactionQuery = transactionQuery.Where(p => p.UserEWallet!.User.PhoneNumber!.ToLower().Trim()
                    .Contains(request.FilterOptions.PhoneNumber.ToLower().Trim()));
            }

            if (!string.IsNullOrWhiteSpace(request.FilterOptions.Email))
            {
                transactionQuery = transactionQuery.Where(p => p.UserEWallet!.User.Email!.ToLower().Trim()
                    .Contains(request.FilterOptions.Email.ToLower().Trim()));
            }
        }

        // Sort
        transactionQuery = request.SortOptions switch
        {
            GetTransactionSortOption.CreatedTimeAscending => transactionQuery.OrderBy(p => p.CreatedTime),
            GetTransactionSortOption.CreatedTimeDescending => transactionQuery.OrderByDescending(p => p.CreatedTime),
            GetTransactionSortOption.AmountAscending => transactionQuery.OrderBy(p => p.Amount),
            GetTransactionSortOption.AmountDescending => transactionQuery.OrderByDescending(p => p.Amount),
            GetTransactionSortOption.TransactionTypeAscending => transactionQuery.OrderBy(p => p.Type),
            GetTransactionSortOption.TransactionTypeDescending => transactionQuery.OrderByDescending(p => p.Type),
            _ => transactionQuery.OrderByDescending(p => p.CreatedTime)
        };

        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<Transaction>()
            .GetPagination(transactionQuery, request.PageNumber, request.PageSize);

        IList<GetTransactionResponse> transactions = new List<GetTransactionResponse>();

        foreach (var transaction in queryPaging.Data)
        {
            var transactionEntity =
                await _unitOfWork.GetRepository<Transaction>().FindAsync(p => p.Id == transaction.Id);

            transactions.Add(_mapper.Map<GetTransactionResponse>(transactionEntity));

            // Map user in transaction
            transactions.Last().User = _mapper.Map<GetUserInTransactionResponse>(transactionEntity!.UserEWallet!.User);
        }

        return new PaginationResponse<GetTransactionResponse>(transactions, queryPaging.PageNumber,
            queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }

    public async Task<GetTransactionDetailResponse> GetTransactionDetail(Guid transactionId)
    {
        var transaction = await _unitOfWork.GetRepository<Transaction>().FindAsync(p => p.Id == transactionId);

        // Check if transaction is null
        if (transaction == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Transaction not found");
        }

        var transactionResponse = _mapper.Map<GetTransactionDetailResponse>(transaction);

        // Map user in transaction
        transactionResponse.User = _mapper.Map<GetUserInTransactionResponse>(transaction.UserEWallet!.User);

        // Map receive user in transaction when transaction type is donate
        if (transaction is { Type: TransactionType.Donate, ReceiveUserEWallet: not null })
        {
            transactionResponse.ReceiveUser =
                _mapper.Map<GetUserInTransactionResponse>(transaction.ReceiveUserEWallet.User);
        }
        
        // Handle WithdrawalForm.UserBankType explicitly
        if (transaction.WithdrawalForm != null && transaction.WithdrawalForm.UserBankType == null)
        {
            transactionResponse.WithdrawalForm!.UserBankType = null;
        }

        return transactionResponse;
    }
    
    public async Task<UserGetTransactionDetailResponse> UserGetTransactionDetail(Guid userId, Guid transactionId)
    {
        var transaction = await _unitOfWork.GetRepository<Transaction>().FindAsync(p => p.Id == transactionId && p.UserEWallet!.UserId == userId);

        // Check if transaction is null
        if (transaction == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Không tìm thấy giao dịch");
        }

        var transactionResponse = _mapper.Map<UserGetTransactionDetailResponse>(transaction);

        // Map user in transaction
        transactionResponse.User = _mapper.Map<GetUserInTransactionResponse>(transaction.UserEWallet!.User);

        // Map receive user in transaction when transaction type is donate
        if (transaction is { Type: TransactionType.Donate, ReceiveUserEWallet: not null })
        {
            transactionResponse.ReceiveUser =
                _mapper.Map<GetUserInTransactionResponse>(transaction.ReceiveUserEWallet.User);
        }
        
        // Handle WithdrawalForm.UserBankType explicitly
        if (transaction.WithdrawalForm != null && transaction.WithdrawalForm.UserBankType == null)
        {
            transactionResponse.WithdrawalForm!.UserBankType = null;
        }

        return transactionResponse;
    }
}
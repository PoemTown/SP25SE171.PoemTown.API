using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.BankTypeRequests;
using PoemTown.Service.BusinessModels.ResponseModels.BankTypeResponses;
using PoemTown.Service.QueryOptions.FilterOptions.BankTypeFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.BankTypeSorts;

namespace PoemTown.Service.Interfaces;

public interface IBankTypeService
{
    Task CreateNewBankType(CreateNewBankTypeRequest request);
    Task UpdateBankType(UpdateBankTypeRequest request);
    Task DeleteBankType(Guid bankTypeId);
    Task<GetBankTypeResponse> GetBankTypeDetail(Guid bankTypeId);
    Task<string> UploadBankTypeImageIcon(IFormFile file);
    Task<IEnumerable<GetBankTypeResponse>> UserGetBankTypes(GetBankTypeFilterOption filter);

    Task<PaginationResponse<GetBankTypeResponse>>
        GetBankTypes(RequestOptionsBase<GetBankTypeFilterOption, GetBankTypeSortOption> request);

    Task CreateUserBankType(Guid userId, CreateUserBankTypeRequest request);
    Task UpdateUserBankType(Guid userId, UpdateUserBankTypeRequest request);
    Task DeleteUserBankType(Guid userBankTypeId);
    Task<GetUserBankTypeResponse> GetUserBankTypeDetail(Guid userBankTypeId);

    Task<PaginationResponse<GetUserBankTypeResponse>> GetUserBankTypes
        (Guid userId, RequestOptionsBase<GetUserBankTypeFilterOption, GetUserBankTypeSortOption> request);
}
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.WithdrawalFormRequests;
using PoemTown.Service.BusinessModels.ResponseModels.WithdrawalFormResponses;
using PoemTown.Service.QueryOptions.FilterOptions.WithdrawalFormFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.WithdrawalFormSorts;

namespace PoemTown.Service.Interfaces;

public interface IWithdrawalFormService
{
    Task<PaginationResponse<GetWithdrawalFormResponse>> GetWithdrawalForms(
        RequestOptionsBase<GetWithdrawalFormFilterOption, GetWithdrawalFormSortOption> request);

    Task<PaginationResponse<GetWithdrawalFormResponse>> GetMyWithdrawalForms(Guid userId,
        RequestOptionsBase<GetWithdrawalFormFilterOption, GetWithdrawalFormSortOption> request);

    Task<GetWithdrawalFormResponse> GetWithdrawalFormDetail(Guid id);
    Task ResolveWithdrawalForm(ResolveWithdrawalFormRequest request);
    Task<string> UploadWithdrawalFormEvidence(IFormFile file);
}
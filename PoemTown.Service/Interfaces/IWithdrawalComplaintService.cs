using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.WithdrawalComplaintRequests;
using PoemTown.Service.BusinessModels.ResponseModels.WithdrawalComplaintResponses;
using PoemTown.Service.QueryOptions.FilterOptions.WithdrawalComplaintFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.WithdrawalComplaintSorts;

namespace PoemTown.Service.Interfaces;

public interface IWithdrawalComplaintService
{
    Task CreateNewWithdrawalComplaint(Guid withdrawalFormId, CreateNewWithdrawalComplaintRequest request);
    Task UpdateWithdrawalComplaint(UpdateWithdrawalComplaintRequest request);
    Task<string> UploadWithdrawalComplaintImage(IFormFile file);
    Task DeleteWithdrawalComplaint(Guid withdrawalComplaintId);
    Task AddImageToUpdateWithdrawalComplaint(Guid withdrawalComplaintId, string image);
    Task DeleteImageFromWithdrawalComplaint(Guid withdrawalComplaintId, Guid withdrawalComplaintImageId);

    Task<PaginationResponse<GetWithdrawalComplaintResponse>> GetWithdrawalComplaints(
        RequestOptionsBase<GetWithdrawalComplaintFilterOption, GetWithdrawalComplaintSortOption> request);

    Task<PaginationResponse<GetWithdrawalComplaintResponse>> GetMyWithdrawalComplaints(Guid userId,
        RequestOptionsBase<GetWithdrawalComplaintFilterOption, GetWithdrawalComplaintSortOption> request);

    Task<GetWithdrawalComplaintResponse> GetWithdrawalComplaintById(Guid withdrawalComplaintId);
    Task ResolveWithdrawalComplaint(Guid withdrawalComplaintId, ResolveWithdrawalComplaintRequest request);
}
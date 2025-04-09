using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.UserRequests;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.QueryOptions.FilterOptions.UserFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.UserSorts;

namespace PoemTown.Service.Interfaces;

public interface IUserService
{
    Task<GetUserProfileResponse> GetMyProfile(Guid userId);
    Task UpdateMyProfile(Guid userId, UpdateMyProfileRequest request);
    Task<string> UploadProfileImage(Guid userId, IFormFile file);
    Task<GetOwnOnlineProfileResponse> GetOwnOnlineProfile(Guid userId);
    Task<GetUserOnlineProfileResponse> GetUserOnlineProfileResponse(Guid? userId, string userName);

    Task<PaginationResponse<GetUsersResponse>>
        GetUserProfiles(RequestOptionsBase<GetUserFilterOption, GetUserSortOption> request);
}
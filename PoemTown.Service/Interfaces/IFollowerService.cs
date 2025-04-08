using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.ResponseModels.FollowerResponses;
using PoemTown.Service.QueryOptions.FilterOptions.FollowerFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.FollowerSorts;

namespace PoemTown.Service.Interfaces;

public interface IFollowerService
{
    Task FollowUserAsync(Guid userId, Guid followedUserId);
    Task UnfollowUserAsync(Guid userId, Guid followerId);

    Task<PaginationResponse<GetFollowersResponse>>
        GetMyFollower(Guid userId, RequestOptionsBase<GetFollowersFilterOption, GetFollowersSortOption> request);

    Task<PaginationResponse<GetFollowersResponse>> GetMyFollowList(Guid userId,
        RequestOptionsBase<GetFollowersFilterOption, GetFollowersSortOption> request);

    Task<PaginationResponse<GetFollowersResponse>>
        GetUserFollower(string userName, RequestOptionsBase<GetFollowersFilterOption, GetFollowersSortOption> request);

    Task<PaginationResponse<GetFollowersResponse>> GetUserFollowList(string userName,
        RequestOptionsBase<GetFollowersFilterOption, GetFollowersSortOption> request);
}
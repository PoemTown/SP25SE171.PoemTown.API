using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.ResponseModels.FollowerResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Events.AnnouncementEvents;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.FollowerFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.FollowerSorts;

namespace PoemTown.Service.Services;

public class FollowerService : IFollowerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public FollowerService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task FollowUserAsync(Guid userId, Guid followedUserId)
    {
        // Check if the followed user exists
        var followedUser = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == followedUserId);
        if (followedUser == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Followed user not found");
        }

        // Check if the user has already followed the followed user
        var follower = await _unitOfWork.GetRepository<Follower>()
            .FindAsync(p => p.FollowUserId == userId && p.FollowedUserId == followedUserId);

        if (follower != null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "You have already followed this user");
        }

        // Check if the user is trying to follow themselves
        if (userId == followedUserId)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "You cannot follow yourself");
        }

        // Create a new follower
        var newFollower = new Follower
        {
            FollowUserId = userId,
            FollowedUserId = followedUserId
        };

        await _unitOfWork.GetRepository<Follower>().InsertAsync(newFollower);
        await _unitOfWork.SaveChangesAsync();

        // Get follower user information
        var followerUser = await _unitOfWork.GetRepository<User>()
            .FindAsync(p => p.Id == userId);

        if (followerUser != null)
        {
            // Announce the new follower to the followed user
            await _publishEndpoint.Publish(new SendUserAnnouncementEvent()
            {
                UserId = followedUser.Id,
                Title = "Người theo dõi mới",
                Content = $"{followerUser.UserName} đã theo dõi bạn",
            });
        }
    }

    public async Task UnfollowUserAsync(Guid userId, Guid followerId)
    {
        var follower = await _unitOfWork.GetRepository<Follower>()
            .FindAsync(p => p.FollowedUserId == followerId && p.FollowUserId == userId);

        // Check if the follower exists
        if (follower == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Follower not found");
        }

        // Check if the user is authorized to unfollow this user
        if (follower.FollowUserId != userId)
        {
            throw new CoreException(StatusCodes.Status401Unauthorized, "You are not authorized to unfollow this user");
        }

        _unitOfWork.GetRepository<Follower>().DeletePermanent(follower);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Get my followers (people who follow me)
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<PaginationResponse<GetFollowersResponse>>
        GetMyFollower(Guid userId, RequestOptionsBase<GetFollowersFilterOption, GetFollowersSortOption> request)
    {
        var followerQuery = _unitOfWork.GetRepository<Follower>().AsQueryable();

        // Get by my followers
        followerQuery = followerQuery.Where(p => p.FollowedUserId == userId);

        // Filter
        if (request.FilterOptions != null)
        {
            if (!string.IsNullOrWhiteSpace(request.FilterOptions.DisplayName))
            {
                followerQuery = followerQuery.Where(p => p.FollowedUser!.DisplayName!
                    .Contains(request.FilterOptions.DisplayName, StringComparison.OrdinalIgnoreCase));
            }
        }

        // Sort
        followerQuery = request.SortOptions switch
        {
            GetFollowersSortOption.Nearest => followerQuery.OrderByDescending(p => p.CreatedTime),
            GetFollowersSortOption.Oldest => followerQuery.OrderBy(p => p.CreatedTime),
            _ => followerQuery.OrderByDescending(p => p.CreatedTime),
        };

        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<Follower>()
            .GetPagination(followerQuery, request.PageNumber, request.PageSize);

        //var followers = _mapper.Map<IList<GetFollowersResponse>>(queryPaging.Data);
        IList<GetFollowersResponse> followers = new List<GetFollowersResponse>();
        foreach (var follower in queryPaging.Data)
        {
            // Get user who is following me
            var user = await _unitOfWork.GetRepository<Follower>()
                .FindAsync(p => p.FollowUserId == follower.FollowUserId && p.FollowedUserId == userId);
            if (user == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
            }

            followers.Add(_mapper.Map<GetFollowersResponse>(user));
            followers.Last().User = _mapper.Map<GetBasicUserInformationResponse>(user.FollowUser);
        }

        return new PaginationResponse<GetFollowersResponse>
        (followers, queryPaging.PageNumber, queryPaging.PageSize, queryPaging.TotalRecords,
            queryPaging.CurrentPageRecords);
    }

    /// <summary>
    /// Get my follow list (people I have followed)
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<PaginationResponse<GetFollowersResponse>> GetMyFollowList(Guid userId,
        RequestOptionsBase<GetFollowersFilterOption, GetFollowersSortOption> request)
    {
        var followerQuery = _unitOfWork.GetRepository<Follower>().AsQueryable();

        // Get my user list that I have followed
        followerQuery = followerQuery.Where(p => p.FollowUserId == userId);

        // Filter
        if (request.FilterOptions != null)
        {
            if (!string.IsNullOrWhiteSpace(request.FilterOptions.DisplayName))
            {
                followerQuery = followerQuery.Where(p => p.FollowedUser!.DisplayName!
                    .Contains(request.FilterOptions.DisplayName, StringComparison.OrdinalIgnoreCase));
            }
        }

        // Sort
        followerQuery = request.SortOptions switch
        {
            GetFollowersSortOption.Nearest => followerQuery.OrderByDescending(p => p.CreatedTime),
            GetFollowersSortOption.Oldest => followerQuery.OrderBy(p => p.CreatedTime),
            _ => followerQuery.OrderByDescending(p => p.CreatedTime),
        };

        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<Follower>()
            .GetPagination(followerQuery, request.PageNumber, request.PageSize);

        //var followers = _mapper.Map<IList<GetFollowersResponse>>(queryPaging.Data);

        IList<GetFollowersResponse> followers = new List<GetFollowersResponse>();
        foreach (var follower in queryPaging.Data)
        {
            // Get user who is followed
            var user = await _unitOfWork.GetRepository<Follower>()
                .FindAsync(p => p.FollowedUserId == follower.FollowedUserId && p.FollowUserId == userId);
            if (user == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
            }

            followers.Add(_mapper.Map<GetFollowersResponse>(user));
            followers.Last().User = _mapper.Map<GetBasicUserInformationResponse>(user.FollowedUser);
        }

        return new PaginationResponse<GetFollowersResponse>
        (followers, queryPaging.PageNumber, queryPaging.PageSize, queryPaging.TotalRecords,
            queryPaging.CurrentPageRecords);
    }
}
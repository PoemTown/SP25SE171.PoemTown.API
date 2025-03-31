using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.LeaderBoards;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Repository.Enums.UserPoems;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.ResponseModels.LeaderBoardDetailResponses;
using PoemTown.Service.BusinessModels.ResponseModels.LeaderBoardResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserLeaderBoardResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.LeaderBoardFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.LeaderBoardSorts;
using PoemTown.Service.Scheduler.LeaderBoardJobs;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Services
{
    public class LeaderBoardService : ILeaderBoardService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public LeaderBoardService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CalculateTopPoemsAsync()
        {
            const int LikeWeight = 1;
            const int CommentWeight = 2;
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddSeconds(-1);

            var poemRepository = _unitOfWork.GetRepository<Poem>();
            var poems = await poemRepository.AsQueryable()
                .Where(p => p.Status == PoemStatus.Posted)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                /*
                .Include(p => p.UserPoemRecordFiles)
                */
                .ToListAsync();

            var poemScores = poems.Select(p =>
            {
                int likeCount = p.Likes?.Count(l => l.CreatedTime >= startOfMonth && l.CreatedTime < endOfMonth) ?? 0;
                int commentCount = p.Comments?.Count(c => c.CreatedTime >= startOfMonth && c.CreatedTime < endOfMonth) ?? 0;
                int score = likeCount * LikeWeight + commentCount * CommentWeight;
                return new { Poem = p, Score = score };
            }).ToList();

            var topPoems = poemScores
               .OrderByDescending(x => x.Score)
               .Take(20)
               .ToList();

            var leaderboardRepository = _unitOfWork.GetRepository<LeaderBoard>();
            var leaderboard = await leaderboardRepository.AsQueryable()
                .Include(lb => lb.PoemLeaderBoards)
                .FirstOrDefaultAsync(lb =>
                lb.Type == LeaderBoardType.Poem &&
                lb.Status == LeaderBoardStatus.InProgress &&
                lb.StartDate == startOfMonth);

            if (leaderboard == null)
            {
                leaderboard = new LeaderBoard
                {
                    Id = Guid.NewGuid(),
                    Type = LeaderBoardType.Poem,
                    StartDate = startOfMonth,
                    EndDate = endOfMonth,
                    Status = LeaderBoardStatus.InProgress,
                    PoemLeaderBoards = new List<PoemLeaderBoard>()
                };
                await leaderboardRepository.InsertAsync(leaderboard);
            }
            else
            {
                // Remove existing leaderboard details.
                var lbDetailRepository = _unitOfWork.GetRepository<PoemLeaderBoard>();
                foreach (var detail in leaderboard.PoemLeaderBoards.ToList())
                {
                    lbDetailRepository.DeletePermanent(detail);
                }
                leaderboard.PoemLeaderBoards.Clear();
                await _unitOfWork.SaveChangesAsync();
            }
            var lbDetailRepo = _unitOfWork.GetRepository<PoemLeaderBoard>();
            int rank = 1;
            foreach (var item in topPoems)
            {
                // Skip items that don't have an AuthorId if needed.
                //if (item.AuthorId == null) continue;

                var detail = new PoemLeaderBoard
                {
                    Id = Guid.NewGuid(),
                    LeaderBoardId = leaderboard.Id,
                    PoemId = item.Poem.Id,
                    Rank = rank  // Set the rank here
                };

                await lbDetailRepo.InsertAsync(detail);
                leaderboard.PoemLeaderBoards.Add(detail);
                rank++;
            }
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task CalculateTopUsersAsync()
        {
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddSeconds(-1);
            var userRepository = _unitOfWork.GetRepository<User>();
            var users = await _unitOfWork.GetRepository<User>().AsQueryable()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.FollowedUser)
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name == "USER"))
                .ToListAsync();


            // Calculate the score for each user by counting new followers within the current month.
            var userScores = users.Select(u =>
            {
                int newFollowers = u.FollowedUser?.Count(f => f.CreatedTime >= startOfMonth && f.CreatedTime < endOfMonth) ?? 0;
                return new { User = u, Score = newFollowers };
            }).ToList();

            // Get the top 20 users by new follower count.
            var topUsers = userScores
                .OrderByDescending(u => u.Score)
                .Take(20)
                .ToList();

            // Retrieve (or create) the current month’s leaderboard for users.
            var leaderboardRepository = _unitOfWork.GetRepository<LeaderBoard>();
            var leaderboard = await leaderboardRepository.AsQueryable()
                .Include(lb => lb.UserLeaderBoards)
                .FirstOrDefaultAsync(lb =>
                    lb.Type == LeaderBoardType.User &&
                    lb.Status == LeaderBoardStatus.InProgress &&
                    lb.StartDate == startOfMonth);
            if (leaderboard == null)
            {
                leaderboard = new LeaderBoard
                {
                    Id = Guid.NewGuid(),
                    Type = LeaderBoardType.User,
                    StartDate = startOfMonth,
                    EndDate = endOfMonth,
                    Status = LeaderBoardStatus.InProgress,
                    UserLeaderBoards = new List<UserLeaderBoard>()
                };
                await leaderboardRepository.InsertAsync(leaderboard);
            }
            else
            {
                // Remove existing user leaderboard entries.
                var userLeaderboardRepo = _unitOfWork.GetRepository<UserLeaderBoard>();
                foreach (var entry in leaderboard.UserLeaderBoards.ToList())
                {
                    userLeaderboardRepo.DeletePermanent(entry);
                }
                leaderboard.UserLeaderBoards.Clear();
                await _unitOfWork.SaveChangesAsync();
            }

            var userLbRepo = _unitOfWork.GetRepository<UserLeaderBoard>();
            int rank = 1;
            foreach (var item in topUsers)
            {
                var userEntry = new UserLeaderBoard
                {
                    Id = Guid.NewGuid(),
                    LeaderBoardId = leaderboard.Id,
                    UserId = item.User.Id,
                    Rank = rank // Set the rank here
                };

                await userLbRepo.InsertAsync(userEntry);
                leaderboard.UserLeaderBoards.Add(userEntry);
                rank++;
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<GetLeaderBoardResponse> GetTopPoemsLeaderBoard(RequestOptionsBase<GetLeaderBoardFilterOption, GetLeaderBoardSortOption> request)
        {
            DateTimeOffset targetDate = request.FilterOptions?.Date ?? DateTimeHelper.SystemTimeNow;

            var leaderboardQuery = _unitOfWork.GetRepository<LeaderBoard>().AsQueryable()
              .Include(lb => lb.PoemLeaderBoards)
                .ThenInclude(detail => detail.Poem)
                    .ThenInclude(p => p.User)
                .Where(lb => lb.Type == LeaderBoardType.Poem
                 && lb.Status == LeaderBoardStatus.InProgress
                 && lb.StartDate.HasValue
                 && lb.StartDate.Value.Month == targetDate.Month
                 && lb.StartDate.Value.Year == targetDate.Year);

            var leaderboard = await leaderboardQuery.FirstOrDefaultAsync();

            if (leaderboard == null)
            {
                return new GetLeaderBoardResponse();
            }

            // Start with the leaderboard details.
            var detailsQuery = leaderboard.PoemLeaderBoards.AsQueryable();

            // Filter by poem name if provided.
            if (!string.IsNullOrWhiteSpace(request.FilterOptions?.Name))
            {
                detailsQuery = detailsQuery.Where(d => d.Poem != null &&
                    d.Poem.Title.Contains(request.FilterOptions.Name, StringComparison.OrdinalIgnoreCase));
            }

            // Sort by rank. Default is ascending (RankAscending), but if sort option is RankDescending, then reverse.
            if (request.SortOptions == GetLeaderBoardSortOption.RankDescending)
            {
                detailsQuery = detailsQuery.OrderByDescending(d => d.Rank);
            }
            else
            {
                detailsQuery = detailsQuery.OrderBy(d => d.Rank);
            }

            detailsQuery = detailsQuery.Take(20);

            var topPoems = detailsQuery.Select(d => new GetLeaderBoardDetailResponse
            {
                Id = d.Id,
                PoemId = d.PoemId,
                LeaderBoardId = d.LeaderBoardId,
                Rank = d.Rank,
                Poem = d.Poem != null ? _mapper.Map<GetPoemResponse>(d.Poem) : null,

            }).ToList();

            var leaderBoardResponse = new GetLeaderBoardResponse
            {
                Id = leaderboard.Id,
                Type = leaderboard.Type,
                StartDate = leaderboard.StartDate,
                EndDate = leaderboard.EndDate,
                Status = leaderboard.Status,
                TopPoems = topPoems,
                TopUsers = null  // This method only returns the poem leaderboard.
            };

            /*  return new PaginationResponse<GetLeaderBoardResponse>(
                  new List<GetLeaderBoardResponse> { leaderBoardResponse },
                  request.PageNumber,
                  request.PageSize,
                  totalRecords: 1,
                  currentPageRecords: 1);*/
            return leaderBoardResponse;
        }

        public async Task<GetLeaderBoardResponse> GetTopUsersLeaderBoard(RequestOptionsBase<GetLeaderBoardFilterOption, GetLeaderBoardSortOption> request)
        {
            // Use the provided date or default to the current system time.
            DateTimeOffset targetDate = request.FilterOptions?.Date ?? DateTimeHelper.SystemTimeNow;

            // Query for the User LeaderBoard for the target month.
            var leaderboardQuery = _unitOfWork.GetRepository<LeaderBoard>().AsQueryable()
                .Include(lb => lb.UserLeaderBoards)
                    .ThenInclude(ulb => ulb.User)
                .Where(lb => lb.Type == LeaderBoardType.User
                             && lb.Status == LeaderBoardStatus.InProgress
                             && lb.StartDate.HasValue
                             && lb.StartDate.Value.Month == targetDate.Month
                             && lb.StartDate.Value.Year == targetDate.Year);

            var leaderboard = await leaderboardQuery.FirstOrDefaultAsync();
            if (leaderboard == null)
            {
                return new GetLeaderBoardResponse();
            }

            // Start with the leaderboard's user leaderboard entries.
            var detailsQuery = leaderboard.UserLeaderBoards.AsQueryable();

            // Filter by user name if provided.
            if (!string.IsNullOrWhiteSpace(request.FilterOptions?.Name))
            {
                detailsQuery = detailsQuery.Where(d => d.User != null &&
                    d.User.DisplayName.Contains(request.FilterOptions.Name, StringComparison.OrdinalIgnoreCase));
            }

            // Sort by rank.
            if (request.SortOptions == GetLeaderBoardSortOption.RankDescending)
            {
                detailsQuery = detailsQuery.OrderByDescending(d => d.Rank);
            }
            else
            {
                detailsQuery = detailsQuery.OrderBy(d => d.Rank);
            }

            // Limit to the top 20 entries.
            detailsQuery = detailsQuery.Take(20);

            // Map the details to GetUserLeaderBoardResponse using AutoMapper.
            var topUsers = detailsQuery.Select(d => new GetUserLeaderBoardResponse
            {
                Id = d.Id,
                UserId = d.UserId,
                LeaderBoardId = d.LeaderBoardId,
                Rank = d.Rank,
                User = d.User != null ? _mapper.Map<GetUserProfileResponse>(d.User) : null
            }).ToList();

            // Create the leaderboard response.
            var leaderBoardResponse = new GetLeaderBoardResponse
            {
                Id = leaderboard.Id,
                Type = leaderboard.Type,
                StartDate = leaderboard.StartDate,
                EndDate = leaderboard.EndDate,
                Status = leaderboard.Status,
                TopUsers = topUsers,
                TopPoems = null // This endpoint is for users.
            };

            // Wrap the response in a pagination response.
            return leaderBoardResponse;
        }
    }
}

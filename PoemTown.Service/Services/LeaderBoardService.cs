using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.LeaderBoards;
using PoemTown.Repository.Enums.UserPoems;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;
        public LeaderBoardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CalculateTopPoemsAsync()
        {
            const int LikeWeight = 1;
            const int CommentWeight = 2;
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var poemRepository = _unitOfWork.GetRepository<Poem>();
            var poems = await poemRepository.AsQueryable()
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .Include(p => p.UserPoemRecordFiles)
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
                .Include(lb => lb.LeaderBoardDetails)
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
                    LeaderBoardDetails = new List<LeaderBoardDetail>()
                };
                await leaderboardRepository.InsertAsync(leaderboard);
            }
            else
            {
                // Remove existing leaderboard details.
                var lbDetailRepository = _unitOfWork.GetRepository<LeaderBoardDetail>();
                foreach (var detail in leaderboard.LeaderBoardDetails.ToList())
                {
                    lbDetailRepository.DeletePermanent(detail);
                }
                leaderboard.LeaderBoardDetails.Clear();
                await _unitOfWork.SaveChangesAsync();
            }
            var lbDetailRepo = _unitOfWork.GetRepository<LeaderBoardDetail>();
            int rank = 1;
            foreach (var item in topPoems)
            {
                // Skip items that don't have an AuthorId if needed.
                //if (item.AuthorId == null) continue;

                var detail = new LeaderBoardDetail
                {
                    Id = Guid.NewGuid(),
                    LeaderBoardId = leaderboard.Id,
                    PoemId = item.Poem.Id,
                    Rank = rank  // Set the rank here
                };

                await lbDetailRepo.InsertAsync(detail);
                leaderboard.LeaderBoardDetails.Add(detail);
                rank++;
            }
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task CalculateTopUsersAsync()
        {
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);
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

    }
}

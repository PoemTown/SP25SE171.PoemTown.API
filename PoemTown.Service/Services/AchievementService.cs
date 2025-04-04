using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Achievements;
using PoemTown.Repository.Enums.LeaderBoards;
using PoemTown.Repository.Enums.UserPoems;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.ResponseModels.AchievementRespponses;
using PoemTown.Service.BusinessModels.ResponseModels.CommentResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.AchievementFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.AchievementSorts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MassTransit;
using PoemTown.Service.Events.AnnouncementEvents;

namespace PoemTown.Service.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        public AchievementService(IUnitOfWork unitOfWork, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }
        public async Task CreateMonthlyAchievementsAsync()
        {
            var now = DateTimeHelper.SystemTimeNow;
            // Find leaderboards that have ended and are still in progress.
            // (You might adjust the filter depending on your logic.)
            var leaderBoardRepo = _unitOfWork.GetRepository<LeaderBoard>();

            // Process Poem Leaderboard
            var poemLeaderboard = await leaderBoardRepo.AsQueryable()
               .Include(lb => lb.PoemLeaderBoards)
               .FirstOrDefaultAsync(lb =>
                   lb.Type == LeaderBoardType.Poem &&
                   lb.Status == LeaderBoardStatus.InProgress &&
                   lb.EndDate <= now);

            if (poemLeaderboard != null)
            {
                var userAchievements = new Dictionary<Guid, List<string>>();
                
                // Create achievements for each leaderboard detail entry.
                foreach (var detail in poemLeaderboard.PoemLeaderBoards)
                {
                    // Format the month/year string (e.g., "3/2025")
                    string monthYear = poemLeaderboard.StartDate?.ToString("M/yyyy", CultureInfo.InvariantCulture) ?? "";
                    string achievementName = $"Top {detail.Rank} bài thơ tháng {monthYear}";
                    string achievementDescription = "Đây là phần thưởng bạn nhận được trong bảng xếp hạng bài thơ";

                    /*var record = detail.Poem?.UserPoemRecordFiles
                        .FirstOrDefault(p => p.Type == UserPoemType.CopyRightHolder && p.PoemId == detail.Poem?.Id);*/

                    var record = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == detail.PoemId); // Nhớ coi xem đúng ko rồi sửa khúc này nha khôi
                    if (record == null || record.UserId == null)
                    {
                        // Skip awarding achievement if no owner or owner UserId is null.
                        continue;
                    }

                    var userIDE = record.UserId.Value;
                    var achievement = new Achievement
                    {
                        Id = Guid.NewGuid(),
                        Name = achievementName,
                        Description = achievementDescription,
                        Type = AchievementType.Poem,
                        Rank = detail.Rank,
                        EarnedDate = now, // or use poemLeaderboard.EndDate if desired
                        UserId = userIDE // if you want to use the Poem's owner,                                                                                                    // you may need to adjust this to the author's Id
                    };

                    var achievementRepo = _unitOfWork.GetRepository<Achievement>();
                    await achievementRepo.InsertAsync(achievement);
                    
                    // Add userId to the dictionary if it doesn't exist
                    if (!userAchievements.ContainsKey(userIDE))
                    {
                        userAchievements[userIDE] = new List<string>();
                    }
                    
                    // Add the achievement name to the user's list
                    userAchievements[userIDE].Add(achievementName);
                }

                // Mark the leaderboard as done.
                poemLeaderboard.Status = LeaderBoardStatus.Done;
                await _unitOfWork.SaveChangesAsync();
                
                // Send announcements to users.
                foreach (var userAchievement in userAchievements)
                {
                    Guid userId = userAchievement.Key;
                    List<string> achievements = userAchievement.Value;

                    foreach (var achievement in achievements)
                    {
                        await _publishEndpoint.Publish(new SendUserAnnouncementEvent
                        {
                            UserId = userId,
                            Title = "Thành tích mới",
                            Content = $"Chúc mừng bạn đã nhận được thành tích: \"{achievement}\"",
                            IsRead = false
                        });
                    }
                }
            }

            // Process User Leaderboard
            var userLeaderboard = await leaderBoardRepo.AsQueryable()
                .Include(lb => lb.UserLeaderBoards)
                .FirstOrDefaultAsync(lb =>
                    lb.Type == LeaderBoardType.User &&
                    lb.Status == LeaderBoardStatus.InProgress &&
                    lb.EndDate <= now);

            if (userLeaderboard != null)
            {
                var userAchievements = new Dictionary<Guid, List<string>>();

                foreach (var userEntry in userLeaderboard.UserLeaderBoards)
                {
                    string monthYear = userLeaderboard.StartDate?.ToString("M/yyyy", CultureInfo.InvariantCulture) ?? "";
                    string achievementName = $"Top {userEntry.Rank} nhà thơ tháng {monthYear}";
                    string achievementDescription = "Đây là phần thưởng bạn nhận được trong bảng xếp hạng nhà thơ";

                    var achievement = new Achievement
                    {
                        Id = Guid.NewGuid(),
                        Name = achievementName,
                        Description = achievementDescription,
                        Type = AchievementType.User,
                        Rank = userEntry.Rank,
                        EarnedDate = now,
                        UserId = userEntry.UserId ?? Guid.Empty
                    };

                    var achievementRepo = _unitOfWork.GetRepository<Achievement>();
                    await achievementRepo.InsertAsync(achievement);
                    
                    // Add userId to the dictionary if it doesn't exist
                    if (userEntry.UserId != null)
                    {
                        if (!userAchievements.ContainsKey(userEntry.UserId.Value))
                        {
                            userAchievements[userEntry.UserId.Value] = new List<string>();
                        }
                    
                        // Add the achievement name to the user's list
                        userAchievements[userEntry.UserId.Value].Add(achievementName);
                    }
                }

                userLeaderboard.Status = LeaderBoardStatus.Done;
                await _unitOfWork.SaveChangesAsync();
                
                // Send announcements to users.
                foreach (var userAchievement in userAchievements)
                {
                    Guid userId = userAchievement.Key;
                    List<string> achievements = userAchievement.Value;

                    foreach (var achievement in achievements)
                    {
                        // Send announcement to each user
                        await _publishEndpoint.Publish(new SendUserAnnouncementEvent
                        {
                            UserId = userId,
                            Title = "Thành tích mới",
                            Content = $"Chúc mừng bạn đã nhận được thành tích: \"{achievement}\"",
                            IsRead = false
                        });
                    }
                }
            }
        }

        public async Task<PaginationResponse<GetAchievementResponse>> GetMyAchievements(Guid userId, RequestOptionsBase<GetAchievementFilterOption, GetAchievementSortOption> request)
        {
            var achievementQuery = _unitOfWork.GetRepository<Achievement>().AsQueryable();

            achievementQuery = achievementQuery.Where(a => a.UserId == userId);

            if (request.IsDelete == true)
            {
                achievementQuery = achievementQuery.Where(p => p.DeletedTime != null);
            }
            else
            {
                achievementQuery = achievementQuery.Where(p => p.DeletedTime == null);
            }

            if (request.FilterOptions != null)
            {
                if (request.FilterOptions.Type != null)
                {
                    achievementQuery = achievementQuery.Where(a => a.Type == request.FilterOptions.Type);
                }

                if (request.FilterOptions.Rank != null)
                {
                    achievementQuery = achievementQuery.Where(a => a.Rank == request.FilterOptions.Rank);
                }
            }

            if (request.SortOptions == GetAchievementSortOption.EarnedDateAscending)
            {
                achievementQuery = achievementQuery.OrderBy(a => a.EarnedDate);
            } else
            {
                achievementQuery = achievementQuery.OrderByDescending(a => a.EarnedDate);
            }

            var queryPaging = await _unitOfWork.GetRepository<Achievement>()
                .GetPagination(achievementQuery, request.PageNumber, request.PageSize);
            var achievements = _mapper.Map<IList<GetAchievementResponse>>(queryPaging.Data);

            return new PaginationResponse<GetAchievementResponse>(achievements, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }
    }
}

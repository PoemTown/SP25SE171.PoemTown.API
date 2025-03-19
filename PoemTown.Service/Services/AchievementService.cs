using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.LeaderBoards;
using PoemTown.Repository.Enums.UserPoems;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AchievementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreateMonthlyAchievementsAsync()
        {
            var now = DateTimeHelper.SystemTimeNow;
            // Find leaderboards that have ended and are still in progress.
            // (You might adjust the filter depending on your logic.)
            var leaderBoardRepo = _unitOfWork.GetRepository<LeaderBoard>();

            // Process Poem Leaderboard
            var poemLeaderboard = await leaderBoardRepo.AsQueryable()
               .Include(lb => lb.LeaderBoardDetails)
               .FirstOrDefaultAsync(lb =>
                   lb.Type == LeaderBoardType.Poem &&
                   lb.Status == LeaderBoardStatus.InProgress &&
                   lb.EndDate <= now);

            if (poemLeaderboard != null)
            {
                // Create achievements for each leaderboard detail entry.
                foreach (var detail in poemLeaderboard.LeaderBoardDetails)
                {
                    // Format the month/year string (e.g., "3/2025")
                    string monthYear = poemLeaderboard.StartDate?.ToString("M/yyyy", CultureInfo.InvariantCulture) ?? "";
                    string achievementName = $"Top {detail.Rank} bài thơ tháng {monthYear}";
                    string achievementDescription = "Đây là phần thưởng bạn nhận được trong bảng xếp hạng bài thơ";

                    var record = detail.Poem?.UserPoemRecordFiles
                        .FirstOrDefault(p => p.Type == UserPoemType.CopyRightHolder && p.PoemId == detail.Poem?.Id);
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
                        EarnedDate = now, // or use poemLeaderboard.EndDate if desired
                        UserId = userIDE // if you want to use the Poem's owner,                                                                                                    // you may need to adjust this to the author's Id
                    };

                    var achievementRepo = _unitOfWork.GetRepository<Achievement>();
                    await achievementRepo.InsertAsync(achievement);
                }

                // Mark the leaderboard as done.
                poemLeaderboard.Status = LeaderBoardStatus.Done;
                await _unitOfWork.SaveChangesAsync();
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
                        EarnedDate = now,
                        UserId = userEntry.UserId ?? Guid.Empty
                    };

                    var achievementRepo = _unitOfWork.GetRepository<Achievement>();
                    await achievementRepo.InsertAsync(achievement);
                }

                userLeaderboard.Status = LeaderBoardStatus.Done;
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}

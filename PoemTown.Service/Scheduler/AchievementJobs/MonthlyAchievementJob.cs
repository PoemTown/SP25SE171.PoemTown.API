using Microsoft.Extensions.Logging;
using PoemTown.Service.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Scheduler.AchievementJobs
{
    public class MonthlyAchievementJob : IJob
    {
        private readonly IAchievementService _achievementService;
        private readonly ILogger<MonthlyAchievementJob> _logger;

        public MonthlyAchievementJob(IAchievementService achievementService, ILogger<MonthlyAchievementJob> logger)
        {
            _achievementService = achievementService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Hello world");
            await _achievementService.CreateMonthlyAchievementsAsync();
        }
    }
}

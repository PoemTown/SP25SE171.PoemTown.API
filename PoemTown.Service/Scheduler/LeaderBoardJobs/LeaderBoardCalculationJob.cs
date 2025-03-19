using Microsoft.Extensions.Logging;
using PoemTown.Service.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Scheduler.LeaderBoardJobs
{
    public class LeaderBoardCalculationJob : IJob
    {
        private readonly ILeaderBoardService _leaderBoardService;
        private readonly ILogger<LeaderBoardCalculationJob> _logger;
        public LeaderBoardCalculationJob(ILeaderBoardService leaderBoardService, ILogger<LeaderBoardCalculationJob> logger)
        {
            _leaderBoardService = leaderBoardService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("CalculationJob is running");
            await _leaderBoardService.CalculateTopPoemsAsync();
            await _leaderBoardService.CalculateTopUsersAsync();
        }

    }
}

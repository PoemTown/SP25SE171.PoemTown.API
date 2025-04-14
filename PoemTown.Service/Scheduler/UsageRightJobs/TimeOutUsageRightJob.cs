using Microsoft.Extensions.Logging;
using PoemTown.Service.Interfaces;
using PoemTown.Service.Scheduler.LeaderBoardJobs;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Scheduler.UsageRightJobs
{
    public class TimeOutUsageRightJob : IJob
    {
        private readonly IUsageRightService _service;
        private readonly ILogger<TimeOutUsageRightJob> _logger;
        public TimeOutUsageRightJob(IUsageRightService service, ILogger<TimeOutUsageRightJob> logger)
        {
            _service = service;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("TimeOutUsageRightJob is running");
            await _service.TimeOutUsageRight();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using Quartz;

namespace PoemTown.Service.Scheduler.DailyMessageJobs;

public class UpdateInUseDailyMessageJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateInUseDailyMessageJob> _logger;
    public UpdateInUseDailyMessageJob(IUnitOfWork unitOfWork,
        ILogger<UpdateInUseDailyMessageJob> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("UpdateInUseDailyMessageJob is running");
        
        // Get all daily messages that are not in use first
        var dailyMessages = await _unitOfWork.GetRepository<DailyMessage>()
            .AsQueryable()
            .Where(p => p.DeletedTime == null)
            .ToListAsync();
        
        // Select all daily messages that are not in use
        var dailyMessageNotInUse = dailyMessages
            .Where(p => p.IsInUse == false)
            .ToList();
        
        // Update all Daily Message IsInUse into false
        foreach (var dailyMessage in dailyMessages)
        {
            dailyMessage.IsInUse = false;
        }
        
        // Update all daily messages
        _unitOfWork.GetRepository<DailyMessage>().UpdateRange(dailyMessages);
        
        // Randomly select one daily message that is not in use and set it to be in use
        var random = new Random();
        var randomMessage = dailyMessageNotInUse[random.Next(dailyMessageNotInUse.Count)];
        
        randomMessage.IsInUse = true;
        
        _unitOfWork.GetRepository<DailyMessage>().Update(randomMessage);
        await _unitOfWork.SaveChangesAsync();
    }
}
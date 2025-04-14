using Microsoft.Extensions.Logging;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Orders;
using PoemTown.Repository.Enums.Transactions;
using PoemTown.Repository.Interfaces;
using Quartz;

namespace PoemTown.Service.Scheduler.PaymentJobs;

public class PaymentTimeOutJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PaymentTimeOutJob> _logger;

    public PaymentTimeOutJob(IUnitOfWork unitOfWork, ILogger<PaymentTimeOutJob> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("PaymentTimeOutJob is running");

        JobDataMap jobDataMap = context.JobDetail.JobDataMap;
        string transactionCode = jobDataMap.GetString("orderCode") ?? "";

        var transaction = await _unitOfWork.GetRepository<Transaction>().FindAsync(p => p.TransactionCode == transactionCode);
        
        // Check if order is null
        if (transaction == null)
        {
            _logger.LogError($"Transaction with id {transactionCode} not found");
            return;
        }

        // Check if order is not in Pending status
        if (transaction.Status != TransactionStatus.Pending)
        {
            _logger.LogError($"Transaction with id {transactionCode} is not in Pending status");
            return;
        }

        transaction.Status = TransactionStatus.Cancelled;

        _unitOfWork.GetRepository<Transaction>().Update(transaction);
        await _unitOfWork.SaveChangesAsync();
    }
}
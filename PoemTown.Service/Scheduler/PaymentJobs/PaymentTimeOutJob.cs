using Microsoft.Extensions.Logging;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Orders;
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
        string orderCode = jobDataMap.GetString("orderCode") ?? "";

        var order = await _unitOfWork.GetRepository<Order>().FindAsync(p => p.OrderCode == orderCode);
        
        // Check if order is null
        if (order == null)
        {
            _logger.LogError($"Order with id {orderCode} not found");
            return;
        }

        // Check if order is not in Pending status
        if (order.Status != OrderStatus.Pending)
        {
            _logger.LogError($"Order with id {orderCode} is not in Pending status");
            return;
        }

        order.Status = OrderStatus.Cancelled;

        _unitOfWork.GetRepository<Order>().Update(order);
        await _unitOfWork.SaveChangesAsync();
    }
}
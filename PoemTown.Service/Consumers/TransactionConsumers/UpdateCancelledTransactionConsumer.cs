using MassTransit;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Transactions;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.Events.TransactionEvents;

namespace PoemTown.Service.Consumers.TransactionConsumers;

public class UpdateCancelledTransactionConsumer : IConsumer<UpdateCancelledTransactionEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpdateCancelledTransactionConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<UpdateCancelledTransactionEvent> context)
    {
        var message = context.Message;
        
        Transaction? transaction = await _unitOfWork.GetRepository<Transaction>().FindAsync(p => p.TransactionCode == message.TransactionCode);
        // Check if order is null
        if (transaction == null)
        {
            throw new Exception("Order not found");
        }
        transaction.Status = TransactionStatus.Cancelled;
        transaction.CancelledDate = DateTimeHelper.SystemTimeNow;
        
        _unitOfWork.GetRepository<Transaction>().Update(transaction);   
        await _unitOfWork.SaveChangesAsync();
    }
}
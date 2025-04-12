using MassTransit;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Announcements;
using PoemTown.Repository.Enums.Transactions;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.Events.AnnouncementEvents;
using PoemTown.Service.Events.TransactionEvents;

namespace PoemTown.Service.Consumers.TransactionConsumers;

public class UpdatePaidTransactionConsumer : IConsumer<UpdatePaidTransactionEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;
    
    public UpdatePaidTransactionConsumer(IUnitOfWork unitOfWork,
        IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task Consume(ConsumeContext<UpdatePaidTransactionEvent> context)
    {
        var message = context.Message;

        // Find the transaction by OrderCode
        var transaction = await _unitOfWork.GetRepository<Transaction>().FindAsync(t => t.TransactionCode == message.TransactionCode);
        
        // Check if transaction is null
        if (transaction == null)
        {
            throw new Exception("Transaction not found");
        }

        // Update transaction details
        transaction.BankCode = message.BankCode;
        transaction.DiscountAmount = message.DiscountAmount;
        transaction.Checksum = message.Checksum;
        transaction.Status = TransactionStatus.Paid;
        transaction.PaidDate = DateTimeHelper.SystemTimeNow;
        transaction.Balance = transaction.UserEWallet!.WalletBalance + message.Amount;
        transaction.AppId = message.AppId;
        transaction.IsAddToWallet = true;

        // Save changes to the database
        _unitOfWork.GetRepository<Transaction>().Update(transaction);
        
        await _unitOfWork.SaveChangesAsync();
        
        // Publish event create announcement
        await _publishEndpoint.Publish(new SendUserAnnouncementEvent()
        {
            Title = "Hóa đơn nạp tiền",
            Content = $"Hóa đơn: {transaction.Description} đã khởi tạo thành công",
            UserId = transaction.UserEWallet.UserId,
            Type = AnnouncementType.Transaction,
            TransactionId = transaction.Id,
            IsRead = false
        });
    }
}
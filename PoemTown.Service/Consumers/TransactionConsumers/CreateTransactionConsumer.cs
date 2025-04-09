using MassTransit;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Announcements;
using PoemTown.Repository.Enums.Transactions;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.AnnouncementRequests;
using PoemTown.Service.Events.AnnouncementEvents;
using PoemTown.Service.Events.TransactionEvents;

namespace PoemTown.Service.Consumers.TransactionConsumers;

public class CreateTransactionConsumer : IConsumer<CreateTransactionEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;
    public CreateTransactionConsumer(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task Consume(ConsumeContext<CreateTransactionEvent> context)
    {
        var message = context.Message;
        
        // Check if order is null
        Order? order = await _unitOfWork.GetRepository<Order>().FindAsync(p => p.Id == message.OrderId);
        /*if(order == null)
        {
            throw new Exception("Order not found");
        }*/
        
        UserEWallet? userEWallet;
        if (order == null)
        {
            userEWallet = await _unitOfWork.GetRepository<UserEWallet>().FindAsync(p => p.Id == message.UserEWalletId);
        }
        // Check if user e-wallet is null
        else
        {
            userEWallet = await _unitOfWork.GetRepository<UserEWallet>().FindAsync(p => p.Id == order.User.EWallet!.Id);
        }

        if(userEWallet == null)
        {
            throw new Exception("User e-wallet not found");
        }
        
        // Create transaction
        Transaction transaction = new Transaction()
        {
            Amount = message.Amount,
            Description = message.Description,
            Order = order ?? null,
            Type = message.Type,
            DiscountAmount = message.DiscountAmount,
            UserEWallet = userEWallet,
            Balance = userEWallet.WalletBalance,
            Status = TransactionStatus.Paid,
            PaidDate = DateTimeHelper.SystemTimeNow,
            TransactionCode = message.TransactionCode,
            IsAddToWallet = message.IsAddToWallet,
        };
        
        await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
        
        // Publish event create announcement
        await _publishEndpoint.Publish(new SendUserAnnouncementEvent()
        {
            Title = String.IsNullOrWhiteSpace(message.AnnouncementTitle) ? "Hóa đơn mua hàng" : message.AnnouncementTitle,
            Content = String.IsNullOrWhiteSpace(message.AnnouncementContent) ? $"Hóa đơn: {transaction.Description} đã khởi tạo thành công" : message.AnnouncementContent,
            UserId = transaction.UserEWallet.UserId,
            Type = AnnouncementType.Transaction,
            TransactionId = transaction.Id,
            IsRead = false
        });
    }
}
using MassTransit;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Orders;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.Events.OrderEvents;

namespace PoemTown.Service.Consumers.OrderConsumers;

public class UpdatePaidOrderAndCreateTransactionConsumer : IConsumer<UpdatePaidOrderAndCreateTransactionEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpdatePaidOrderAndCreateTransactionConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<UpdatePaidOrderAndCreateTransactionEvent> context)
    {
        var message = context.Message;

        var order = await _unitOfWork.GetRepository<Order>().FindAsync(p => p.OrderCode == message.OrderCode);
        // Check if order is null
        if (order == null)
        {
            throw new Exception("Order not found");
        }

        if (order.Status != OrderStatus.Pending)
        {
            return;
        }
        // Update order status to Paid
        order.Status = OrderStatus.Paid;
        order.PaidDate = DateTimeHelper.SystemTimeNow;
        _unitOfWork.GetRepository<Order>().Update(order);
        
        // Get user e-wallet
        UserEWallet? userEWallet = await _unitOfWork.GetRepository<UserEWallet>().FindAsync(p => p.UserId == order.UserId);
        if(userEWallet == null)
        {
            throw new Exception("User e-wallet not found");
        }
        
        // Create transaction
        order.Transaction = new()
        {
            Order = order,
            Description = order.OrderDescription,
            BankCode = message.BankCode,
            AppId = message.AppId,
            Amount = (decimal)message.Amount!,
            DiscountAmount = message.DiscountAmount,
            UserEWallet = userEWallet,
            Checksum = message.Checksum,
            Balance = userEWallet.WalletBalance
        };
        
        await _unitOfWork.GetRepository<Transaction>().InsertAsync(order.Transaction);
        await _unitOfWork.SaveChangesAsync();
    }
}
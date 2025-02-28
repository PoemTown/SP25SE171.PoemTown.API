using MassTransit;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Orders;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.PaymentRequests;
using PoemTown.Service.Events.OrderEvents;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Services;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public PaymentService(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task HandleCallbackPaymentSuccessAsync(HandleCallbackPaymentRequest request)
    {
        Order? order = await _unitOfWork.GetRepository<Order>().FindAsync(p => p.OrderCode == request.OrderCode);
        // Check if order is null
        if (order == null)
        {
            throw new Exception("Order not found");
        }
        // Check if order status is not pending
        if(order.Status != OrderStatus.Pending)
        {
            throw new Exception("Order status is not pending");
        }
        
        UserEWallet? userEWallet = await _unitOfWork.GetRepository<UserEWallet>().FindAsync(p => p.UserId == order.UserId);
        // Check if user e-wallet is null
        if(userEWallet == null)
        {
            userEWallet = new UserEWallet()
            {
                UserId = order.UserId,
                WalletBalance = 0
            };
            await _unitOfWork.GetRepository<UserEWallet>().InsertAsync(userEWallet);
            await _unitOfWork.SaveChangesAsync();
        }
        
        // Wallet balance is increased by the amount of the order
        userEWallet.WalletBalance += (decimal)request.Amount!;
        
        _unitOfWork.GetRepository<UserEWallet>().Update(userEWallet);
        await _unitOfWork.SaveChangesAsync();
        
        // Update order status to Paid and create transaction
        var updatePaidOrderAndCreateTransactionEvent = new UpdatePaidOrderAndCreateTransactionEvent()
        {
            OrderCode = request.OrderCode,
            BankCode = request.BankCode,
            AppId = request.AppId,
            Amount = request.Amount,
            DiscountAmount = request.DiscountAmount,
            Checksum = request.Checksum
        };
        
        await _publishEndpoint.Publish(updatePaidOrderAndCreateTransactionEvent);
    }
    
    public async Task HandleCallbackPaymentCancelledAsync(HandleCallbackPaymentRequest request)
    {
        Order? order = await _unitOfWork.GetRepository<Order>().FindAsync(p => p.OrderCode == request.OrderCode);
        // Check if order is null
        if (order == null)
        {
            throw new Exception("Order not found");
        }
        // Check if order status is not pending
        if(order.Status != OrderStatus.Pending)
        {
            throw new Exception("Order status is not pending");
        }
        
        // Update order status to Cancelled
        var updateCancelledOrderEvent = new UpdateCancelledOrderEvent()
        {
            OrderCode = request.OrderCode,
        };
        
        await _publishEndpoint.Publish(updateCancelledOrderEvent);
    }
}
using MassTransit;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Orders;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.Events.OrderEvents;

namespace PoemTown.Service.Consumers.OrderConsumers;

public class UpdateCancelledOrderConsumer : IConsumer<UpdateCancelledOrderEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpdateCancelledOrderConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task Consume(ConsumeContext<UpdateCancelledOrderEvent> context)
    {
        var message = context.Message;
        
        Order? order = await _unitOfWork.GetRepository<Order>().FindAsync(p => p.OrderCode == message.OrderCode);
        // Check if order is null
        if (order == null)
        {
            throw new Exception("Order not found");
        }
        order.Status = OrderStatus.Cancelled;
        order.CancelledDate = DateTimeHelper.SystemTimeNow;
        
        _unitOfWork.GetRepository<Order>().Update(order);   
        await _unitOfWork.SaveChangesAsync();
    }
}
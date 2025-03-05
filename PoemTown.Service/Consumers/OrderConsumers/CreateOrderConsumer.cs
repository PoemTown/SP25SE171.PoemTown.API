using MassTransit;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Transactions;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.Events.OrderEvents;
using PoemTown.Service.Events.TransactionEvents;

namespace PoemTown.Service.Consumers.OrderConsumers;

public class CreateOrderConsumer : IConsumer<CreateOrderEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;
    public CreateOrderConsumer(IUnitOfWork unitOfWork,
        IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task Consume(ConsumeContext<CreateOrderEvent> context)
    {
        var message = context.Message;
        
        var masterTemplate = await _unitOfWork.GetRepository<MasterTemplate>()
            .FindAsync(p => p.Id == message.ItemId);

        // Check if master template exists
        if (masterTemplate == null)
        {
            throw new Exception("Master template not found");
        }
        
        // Create order
        var order = new Order()
        {
            Id = new Guid(),
            Amount = message.Amount,
            OrderCode = message.OrderCode,
            OrderDescription = message.OrderDescription,
            Status = message.Status,
            Type = message.Type,
            PaidDate = message.PaidDate,
            UserId = message.UserId
        };
        await _unitOfWork.GetRepository<Order>().InsertAsync(order);
        
        // Create order details
        var orderDetails = new List<OrderDetail>();
        orderDetails.Add(
            new()
        {
            Order = order,
            ItemPrice = masterTemplate.Price,
            ItemQuantity = 1,
            MasterTemplate = masterTemplate
        });
        
        await _unitOfWork.GetRepository<OrderDetail>().InsertRangeAsync(orderDetails);
        await _unitOfWork.SaveChangesAsync();
        
                
        // Create transaction event
        var createTransactionEvent = new CreateTransactionEvent()
        {
            Amount = message.Amount,
            Description = $"Thanh toán {message.Amount}VND cho đơn hàng: {order.OrderCode}",
            OrderId = order.Id,
            Type = (TransactionType) order.Type,
            DiscountAmount = message.DiscountAmount
        };
        
        await _publishEndpoint.Publish(createTransactionEvent);
    }
}
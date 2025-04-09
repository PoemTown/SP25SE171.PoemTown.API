using MassTransit;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Orders;
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
        
        var orderDetails = new List<OrderDetail>();
        switch (message.Type)
        {
            // Create order details as poems
            case OrderType.Poems:
                var saleVerion = await _unitOfWork.GetRepository<SaleVersion>().FindAsync(p => p.Id == message.SaleVersionId);
                
                // Check if poem is null
                if (saleVerion == null)
                {
                    throw new Exception("saleVerion not found");
                }
                orderDetails.Add(new()
                {
                    Order = order,
                    ItemPrice = saleVerion.Price,
                    ItemQuantity = 1,
                    SaleVersion = saleVerion
                });
                break;
            // Create order details as record files
            case OrderType.RecordFiles:
                var recordFile = await _unitOfWork.GetRepository<RecordFile>().FindAsync(p => p.Id == message.RecordFileId);
                
                // Check if record file is null
                if (recordFile == null)
                {
                    throw new Exception("Record file not found");
                }
                orderDetails.Add(new()
                {
                    Order = order,
                    ItemPrice = recordFile.Price,
                    ItemQuantity = 1,
                    RecordFile = recordFile
                });
                break;
            // Create order details as master templates
            case OrderType.MasterTemplates:
                var masterTemplate = await _unitOfWork.GetRepository<MasterTemplate>().FindAsync(p => p.Id == message.MasterTemplateId);
                
                // Check if master template is null
                if (masterTemplate == null)
                {
                    throw new Exception("Master template not found");
                }
                orderDetails.Add(new()
                {
                    Order = order,
                    ItemPrice = masterTemplate.Price,
                    ItemQuantity = 1,
                    MasterTemplate = masterTemplate
                });
                break;
            case OrderType.EWalletDeposit:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        await _unitOfWork.GetRepository<OrderDetail>().InsertRangeAsync(orderDetails);
        await _unitOfWork.SaveChangesAsync();
        
                
        // Create transaction event
        var createTransactionEvent = new CreateTransactionEvent()
        {
            Amount = message.Amount,
            Description = $"Thanh toán {message.Amount}VND cho đơn hàng: {order.OrderCode}",
            OrderId = order.Id,
            Type = (TransactionType) order.Type,
            DiscountAmount = message.DiscountAmount,
            TransactionCode = order.OrderCode
        };
        
        await _publishEndpoint.Publish(createTransactionEvent);
    }
}
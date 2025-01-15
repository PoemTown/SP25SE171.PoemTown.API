using MassTransit;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.Events.CollectionEvents;

namespace PoemTown.Service.Consumers.CollectionConsumers;

public class CreateDefaultCollectionConsumer : IConsumer<CreateDefaultCollectionEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateDefaultCollectionConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<CreateDefaultCollectionEvent> context)
    {
        var message = context.Message;
        var user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == message.UserId);
        
        if(user == null)
        {
            throw new Exception("User not found");
        }
        
        //Check if user don't have any collection then create default collection
        var userCollections = await _unitOfWork.GetRepository<Collection>()
            .AsQueryable()
            .AnyAsync(p => p.UserId == message.UserId);
        if(userCollections)
        {
            return;
        }
        
        var collection = new Collection()
        {
            UserId = message.UserId,
            CollectionName = "Bộ sưu tập mặc định",
            CollectionDescription = "Bộ sưu tập được khởi tạo mặc định bởi hệ thống",
            IsDefault = true
        };
        
        await _unitOfWork.GetRepository<Collection>().InsertAsync(collection);
        await _unitOfWork.SaveChangesAsync();
    }
}
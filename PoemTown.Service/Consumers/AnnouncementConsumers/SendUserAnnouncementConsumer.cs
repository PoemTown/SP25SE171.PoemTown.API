using MassTransit;
using Microsoft.AspNetCore.SignalR;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.Events.AnnouncementEvents;
using PoemTown.Service.SignalR;
using PoemTown.Service.SignalR.IReceiveClients;
using PoemTown.Service.SignalR.ReceiveClientModels.AnnouncementClientModels;

namespace PoemTown.Service.Consumers.AnnouncementConsumers;

public class SendUserAnnouncementConsumer : IConsumer<SendUserAnnouncementEvent>
{
    private readonly IHubContext<AnnouncementHub, IAnnouncementClient> _hubContext;
    private readonly IUnitOfWork _unitOfWork;

    public SendUserAnnouncementConsumer(IHubContext<AnnouncementHub, IAnnouncementClient> hubContext,
        IUnitOfWork unitOfWork)
    {
        _hubContext = hubContext;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<SendUserAnnouncementEvent> context)
    {
        var message = context.Message;

        // Check if user is null
        var user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == message.UserId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        Guid announcementId = Guid.NewGuid();
        DateTimeOffset createdTime = DateTimeHelper.SystemTimeNow;

        // Create announcement
        var announcement = new Announcement()
        {
            Id = announcementId,
            Title = message.Title,
            Content = message.Content,
            UserId = message.UserId,
            IsRead = message.IsRead,
            CreatedTime = createdTime
        };

        await _unitOfWork.GetRepository<Announcement>().InsertAsync(announcement);
        await _unitOfWork.SaveChangesAsync();

        // Send SignalR if user is online
        var connectionId = AnnouncementHub.GetConnectionId(message.UserId);
        if (connectionId != string.Empty)
        {
            await _hubContext.Clients.Client(connectionId).ReceiveAnnouncement(new CreateNewAnnouncementClientModel()
            {
                Id = announcementId,
                Title = message.Title,
                Content = message.Content,
                IsRead = message.IsRead,
                CreatedTime = createdTime
            });
        }
    }
}
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

public class UpdateAndSendUserAnnouncementConsumer : IConsumer<UpdateAndSendUserAnnouncementEvent>
{
    private readonly IHubContext<AnnouncementHub, IAnnouncementClient> _hubContext;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAndSendUserAnnouncementConsumer(IHubContext<AnnouncementHub, IAnnouncementClient> hubContext,
        IUnitOfWork unitOfWork)
    {
        _hubContext = hubContext;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<UpdateAndSendUserAnnouncementEvent> context)
    {
        var message = context.Message;

        // Check if user is null
        var user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == message.UserId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        // Update announcement
        var announcement = await _unitOfWork.GetRepository<Announcement>()
            .FindAsync(p => p.UserId == user.Id && p.Type == message.Type);
        
        // If announcement is null, create a new one
        if (announcement == null)
        {
            announcement = new Announcement
            {
                UserId = message.UserId,
                Type = message.Type,
                Title = message.Title,
                Content = message.Content,
                IsRead = message.IsRead ?? false,
                CreatedTime = DateTimeHelper.SystemTimeNow
            };
            await _unitOfWork.GetRepository<Announcement>().InsertAsync(announcement);
        }
        
        // If announcement is not null, update it
        else
        {
            announcement.Title = message.Title;
            announcement.Content = message.Content;
            announcement.IsRead = message.IsRead;
            announcement.CreatedTime = DateTimeHelper.SystemTimeNow;

            _unitOfWork.GetRepository<Announcement>().Update(announcement);
        }

        await _unitOfWork.SaveChangesAsync();

        // Send SignalR if user is online
        var connectionId = AnnouncementHub.GetConnectionId(message.UserId);
        if (connectionId != string.Empty)
        {
            await _hubContext.Clients.Client(connectionId).ReceiveAnnouncement(new CreateNewAnnouncementClientModel()
            {
                Id = announcement.Id,
                Title = announcement.Title,
                Content = announcement.Content,
                IsRead = announcement.IsRead,
                CreatedTime = announcement.CreatedTime
            });
        }
    }
}
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
        var announcement = await _unitOfWork.GetRepository<Announcement>().FindAsync(p => p.Id == message.AnnouncementId);
        if (announcement == null)
        {
            throw new Exception("Announcement not found");
        }

        announcement.Title = message.Title;
        announcement.Content = message.Content;
        announcement.IsRead = message.IsRead;
        announcement.CreatedTime = DateTimeHelper.SystemTimeNow;

        _unitOfWork.GetRepository<Announcement>().Update(announcement);
        await _unitOfWork.SaveChangesAsync();

        // Send SignalR if user is online
        var connectionId = AnnouncementHub.GetConnectionId(message.UserId);
        if (connectionId != string.Empty)
        {
            await _hubContext.Clients.Client(connectionId).ReceiveAnnouncement(new CreateNewAnnouncementClientModel()
            {
                Id = message.AnnouncementId,
                Title = message.Title,
                Content = message.Content,
                IsRead = message.IsRead,
                CreatedTime = announcement.CreatedTime
            });
        }
    }
}
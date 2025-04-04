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

public class SendBulkUserAnnouncementConsumer : IConsumer<SendBulkUserAnnouncementEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<AnnouncementHub, IAnnouncementClient> _hubContext;

    public SendBulkUserAnnouncementConsumer(IUnitOfWork unitOfWork,
        IHubContext<AnnouncementHub, IAnnouncementClient> hubContext)
    {
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<SendBulkUserAnnouncementEvent> context)
    {
        var message = context.Message;
        var createdTime = DateTimeHelper.SystemTimeNow;
        var announcements = new List<Announcement>();

        if (message.UserIds != null)
        {
            announcements.AddRange(message.UserIds.Select(userId => new Announcement
            {
                Id = Guid.NewGuid(),
                Title = message.Title,
                Content = message.Content,
                UserId = userId,
                IsRead = message.IsRead,
                CreatedTime = createdTime
            }));
        }

        await _unitOfWork.GetRepository<Announcement>().InsertRangeAsync(announcements);
        await _unitOfWork.SaveChangesAsync();

        // Send SignalR announcements
        foreach (var announcement in announcements)
        {
            var connectionId = AnnouncementHub.GetConnectionId(announcement.UserId);
            if (!string.IsNullOrEmpty(connectionId))
            {
                await _hubContext.Clients.Client(connectionId).ReceiveAnnouncement(new CreateNewAnnouncementClientModel
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
}
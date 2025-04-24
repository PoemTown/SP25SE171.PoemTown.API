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
    private readonly IPublishEndpoint _publishEndpoint;
    public SendBulkUserAnnouncementConsumer(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<SendBulkUserAnnouncementEvent> context)
    {
        var message = context.Message;
        
        // Initialize the list of announcement Events
        var announcementsEvents = new List<SendUserAnnouncementEvent>();

        if (message.UserIds != null)
        {
            announcementsEvents.AddRange(message.UserIds.Select(userId => new SendUserAnnouncementEvent
            {
                Title = message.Title,
                Content = message.Content,
                UserId = userId,
                IsRead = message.IsRead,
                Type = message.Type,
                ReportId = message.ReportId,
                CollectionId = message.CollectionId,
                PoemId = message.PoemId,
                CommentId = message.CommentId,
                LikeId = message.LikeId,
                TransactionId = message.TransactionId,
                AchievementId = message.AchievementId,
                PoemLeaderboardId = message.PoemLeaderboardId,
                UserLeaderboardId = message.UserLeaderboardId,
                RecordFileId = message.RecordFileId,
                FollowerId = message.FollowerId,
                WithdrawalFormId = message.WithdrawalFormId,
                SystemAnnouncementId = message.SystemAnnouncementId
            }));
        }

        // publish the announcement events
        foreach (var announcementEvent in announcementsEvents)
        {
            await _publishEndpoint.Publish(announcementEvent);
        }


        /*await _unitOfWork.GetRepository<Announcement>().InsertRangeAsync(announcements);
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
        }*/
    }
}
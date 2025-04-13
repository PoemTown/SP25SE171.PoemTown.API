using MassTransit;
using Microsoft.AspNetCore.SignalR;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Announcements;
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
            .FindAsync(p => p.UserId == user.Id && p.Id == message.Id);

        // If announcement is null, throw exception
        if (announcement == null)
        {
            throw new Exception("Announcement not found");
        }

        // If announcement is not null, update it
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
                Id = announcement.Id,
                Title = announcement.Title,
                Content = announcement.Content,
                IsRead = announcement.IsRead,
                CreatedTime = announcement.CreatedTime,
                Type = announcement.Type,
                ReportId = announcement.ReportId,
                CollectionId = announcement.CollectionId,
                PoemId = announcement.PoemId,
                CommentId = announcement.CommentId,
                LikeId = announcement.LikeId,
                TransactionId = announcement.TransactionId,
                AchievementId = announcement.AchievementId,
                PoemLeaderboardId = announcement.PoemLeaderboardId,
                UserLeaderboardId = announcement.UserLeaderboardId,
                RecordFileId = announcement.RecordFileId,
                FollowerId = announcement.FollowerId,
                WithdrawalFormId = announcement.WithdrawalFormId
            });
        }
    }
}
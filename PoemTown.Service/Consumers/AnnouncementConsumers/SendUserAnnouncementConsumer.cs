using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Announcements;
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
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<SendUserAnnouncementConsumer> _logger;

    public SendUserAnnouncementConsumer(IHubContext<AnnouncementHub, IAnnouncementClient> hubContext,
        IUnitOfWork unitOfWork,
        IPublishEndpoint publishEndpoint,
        ILogger<SendUserAnnouncementConsumer> logger)
    {
        _hubContext = hubContext;
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
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

        // Check if exist announcement
        var announcementQuery = _unitOfWork.GetRepository<Announcement>()
            .AsQueryable()
            .Where(p => p.UserId == user.Id && p.Type == message.Type);


        // Some of announcement that could be updated and override
        bool isUpdate = false;
        bool isExist = false;

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
            CreatedTime = createdTime,
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
        };

        switch (message.Type)
        {
            case AnnouncementType.Comment:
                isUpdate = true;

                isExist = await announcementQuery.AnyAsync(p => p.PoemId == message.PoemId);
                if (isExist)
                {
                    var existAnnouncement =
                        await announcementQuery.FirstOrDefaultAsync(p => p.PoemId == message.PoemId);
                    announcement.Id = existAnnouncement!.Id;
                }

                break;
            case AnnouncementType.Like:
                isUpdate = true;

                isExist = await announcementQuery.AnyAsync(p => p.PoemId == message.PoemId);
                if (isExist)
                {
                    var existAnnouncement =
                        await announcementQuery.FirstOrDefaultAsync(p => p.PoemId == message.PoemId);
                    announcement.Id = existAnnouncement!.Id;
                }

                break;
            case AnnouncementType.PoemLeaderboard:
                isUpdate = true;

                isExist = await announcementQuery.AnyAsync(p => p.PoemId == message.PoemId);
                if (isExist)
                {
                    var existAnnouncement = await announcementQuery.FirstOrDefaultAsync(p => p.PoemId == message.PoemId);
                    announcement.Id = existAnnouncement!.Id;
                    announcement.PoemLeaderboardId = message.PoemLeaderboardId;
                }

                break;
            case AnnouncementType.UserLeaderboard:
                isUpdate = true;

                isExist = await announcementQuery.AnyAsync();
                if (isExist)
                {
                    var existAnnouncement = await announcementQuery.FirstOrDefaultAsync();
                    announcement.Id = existAnnouncement!.Id;
                    announcement.UserLeaderboardId = message.UserLeaderboardId;
                }

                break;

            /*case AnnouncementType.Report:
                isUpdate = false;

                isExist = await announcementQuery.AnyAsync(p => p.ReportId == message.ReportId);
                break;
            case AnnouncementType.Collection:
                isUpdate = false;

                isExist = await announcementQuery.AnyAsync(p => p.CollectionId == message.CollectionId);
                break;
            case AnnouncementType.Poem:
                isUpdate = false;

                isExist = await announcementQuery.AnyAsync(p => p.PoemId == message.PoemId);
                break;

            case AnnouncementType.Transaction:
                isUpdate = false;

                isExist = await announcementQuery.AnyAsync(p => p.TransactionId == message.TransactionId);
                break;
            case AnnouncementType.Achievement:
                isUpdate = false;

                isExist = await announcementQuery.AnyAsync(p => p.AchievementId == message.AchievementId);
                break;*/
            default:
                break;
        }

        if (isExist && isUpdate)
        {
            await _publishEndpoint.Publish(new UpdateAndSendUserAnnouncementEvent()
            {
                Id = announcement.Id,
                Title = message.Title,
                Content = message.Content,
                Type = message.Type,
                UserId = message.UserId,
                IsRead = message.IsRead,
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
            });
            return;
        }

        await _unitOfWork.GetRepository<Announcement>().InsertAsync(announcement);
        await _unitOfWork.SaveChangesAsync();

        Guid? followerUserId = null;
        
        if(message.FollowerId != null)
        {
            var follower = await _unitOfWork.GetRepository<Follower>()
                .FindAsync(p => p.Id == message.FollowerId);
            if (follower != null)
            {
                followerUserId = follower.FollowUserId;
            }
        }
        
        // Send SignalR if user is online
        var connectionId = AnnouncementHub.GetConnectionId(message.UserId);
        if (connectionId != string.Empty)
        {
            await _hubContext.Clients.Client(connectionId).ReceiveAnnouncement(new CreateNewAnnouncementClientModel()
            {
                Id = announcement.Id,
                Title = message.Title,
                Content = message.Content,
                IsRead = message.IsRead,
                CreatedTime = createdTime,
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
                FollowerUserId = followerUserId,
            });
        }
    }
}
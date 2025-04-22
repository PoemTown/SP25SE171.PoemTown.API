using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Accounts;
using PoemTown.Repository.Enums.Announcements;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.AnnouncementRequests;
using PoemTown.Service.BusinessModels.ResponseModels.AnnouncementResponses;
using PoemTown.Service.Events.AnnouncementEvents;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.AnnouncementFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.AnnouncementSorts;
using PoemTown.Service.SignalR;
using PoemTown.Service.SignalR.IReceiveClients;
using PoemTown.Service.SignalR.ReceiveClientModels.AnnouncementClientModels;

namespace PoemTown.Service.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IHubContext<AnnouncementHub, IAnnouncementClient> _hubContext;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public AnnouncementService(IHubContext<AnnouncementHub, IAnnouncementClient> hubContext,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IPublishEndpoint publishEndpoint)
    {
        _hubContext = hubContext;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task SendAnnouncementAsync(CreateNewAnnouncementRequest request)
    {
        var connectionId = AnnouncementHub.GetConnectionId(request.UserId);

        Guid announcementId = Guid.NewGuid();
        DateTimeOffset createdTime = DateTimeHelper.SystemTimeNow;

        // Send SignalR if user is online
        if (connectionId != string.Empty)
        {
            await _hubContext.Clients.Client(connectionId).ReceiveAnnouncement(new CreateNewAnnouncementClientModel()
            {
                Id = announcementId,
                Title = request.Title,
                Content = request.Content,
                IsRead = request.IsRead,
                CreatedTime = createdTime
            });
        }

        // Check if user exists
        User? user = await _unitOfWork.GetRepository<User>().FindAsync(u => u.Id == request.UserId);
        if (user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }

        await _unitOfWork.GetRepository<Announcement>().InsertAsync(new Announcement
        {
            Id = announcementId,
            Title = request.Title,
            Content = request.Content,
            UserId = request.UserId,
            CreatedTime = createdTime
        });

        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task AdminSendAnnouncementAsync(CreateSystemAnnouncementRequest request)
    {
        // Get all user ids
        var userIds = await _unitOfWork.GetRepository<User>()
            .AsQueryable()
            .Where(u => u.DeletedTime == null && u.Status == AccountStatus.Active)
            .Select(u => u.Id)
            .ToListAsync();
        
        await _publishEndpoint.Publish(new SendBulkUserAnnouncementEvent()
        {
            Title = request.Title ?? "Thông báo mới từ hệ thống",
            Content = request.Content,
            IsRead = false,
            UserIds = userIds,
            Type = AnnouncementType.System,
        });
    }

    public async Task<PaginationResponse<GetAnnouncementResponse>> GetUserAnnouncementsAsync(Guid userId,
        RequestOptionsBase<GetAnnouncementFilterOption, GetAnnouncementSortOption> request)
    {
        var announcementQuery = _unitOfWork.GetRepository<Announcement>().AsQueryable();

        announcementQuery = announcementQuery.Where(p => p.UserId == userId);

        // Filter
        if (request.FilterOptions != null)
        {
            if (request.FilterOptions.IsRead != null)
            {
                announcementQuery = announcementQuery.Where(p => p.IsRead == request.FilterOptions.IsRead);
            }

            if (request.FilterOptions.Type != null)
            {
                announcementQuery = announcementQuery.Where(p => p.Type == request.FilterOptions.Type);
            }
        }

        // Sort
        announcementQuery = request.SortOptions switch
        {
            GetAnnouncementSortOption.CreatedtimeAscending => announcementQuery.OrderBy(p => p.CreatedTime),
            GetAnnouncementSortOption.CreatedtimeDescending => announcementQuery.OrderByDescending(p => p.CreatedTime),
            _ => announcementQuery.OrderByDescending(p => p.CreatedTime)
        };

        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<Announcement>()
            .GetPagination(announcementQuery, request.PageNumber, request.PageSize);

        var announcements = _mapper.Map<IList<GetAnnouncementResponse>>(queryPaging.Data);

        return new PaginationResponse<GetAnnouncementResponse>(
            announcements, queryPaging.PageNumber,
            request.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }


    public async Task DeleteAnnouncementAsync(Guid userId, Guid announcementId)
    {
        var announcement = await _unitOfWork.GetRepository<Announcement>()
            .FindAsync(a => a.Id == announcementId && a.UserId == userId);

        // Check if announcement exists
        if (announcement == null)
        {
            throw new CoreException(StatusCodes.Status404NotFound, "Announcement not found");
        }

        _unitOfWork.GetRepository<Announcement>().Delete(announcement);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAllUserAnnouncementsAsync(Guid userId)
    {
        var announcements = await _unitOfWork.GetRepository<Announcement>()
            .AsQueryable()
            .Where(a => a.UserId == userId)
            .ToListAsync();

        // Check if announcements exist
        if (announcements.Count == 0)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "No announcements found");
        }

        _unitOfWork.GetRepository<Announcement>().DeleteRange(announcements);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PaginationResponse<GetAnnouncementResponse>> 
        GetSystemAnnouncements(RequestOptionsBase<GetSystemAnnouncementFilterOption, GetSystemAnnouncementSortOption> request)
    {
        var announcementQuery = _unitOfWork.GetRepository<Announcement>().AsQueryable();

        announcementQuery = announcementQuery.Where(p => p.Type == AnnouncementType.System);

        // Filter
        if (request.FilterOptions != null)
        {
            /*if (request.FilterOptions.IsRead != null)
            {
                announcementQuery = announcementQuery.Where(p => p.IsRead == request.FilterOptions.IsRead);
            }

            if (request.FilterOptions.Type != null)
            {
                announcementQuery = announcementQuery.Where(p => p.Type == request.FilterOptions.Type);
            }*/
        }

        // Sort
        announcementQuery = request.SortOptions switch
        {
            GetSystemAnnouncementSortOption.CreatedtimeAscending => announcementQuery.OrderBy(p => p.CreatedTime),
            GetSystemAnnouncementSortOption.CreatedtimeDescending => announcementQuery.OrderByDescending(p => p.CreatedTime),
            _ => announcementQuery.OrderByDescending(p => p.CreatedTime)
        };

        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<Announcement>()
            .GetPagination(announcementQuery, request.PageNumber, request.PageSize);

        var announcements = _mapper.Map<IList<GetAnnouncementResponse>>(queryPaging.Data);

        return new PaginationResponse<GetAnnouncementResponse>(
            announcements, queryPaging.PageNumber,
            request.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }
    
    public async Task UpdateAnnouncementToRead(Guid userId, Guid announcementId)
    {
        var announcement = await _unitOfWork.GetRepository<Announcement>()
            .FindAsync(a => a.Id == announcementId && a.UserId == userId);

        // Check if announcement exists
        if (announcement == null)
        {
            throw new CoreException(StatusCodes.Status404NotFound, "Announcement not found");
        }

        announcement.IsRead = true;

        _unitOfWork.GetRepository<Announcement>().Update(announcement);
        await _unitOfWork.SaveChangesAsync();

        // Send changes to signalR if connection is exists
        var connectionId = AnnouncementHub.GetConnectionId(userId);
        if (connectionId != string.Empty)
        {
            await _hubContext.Clients.Client(connectionId).ReceiveAnnouncement(new CreateNewAnnouncementClientModel()
            {
                Id = announcement.Id,
                Title = announcement.Title,
                Content = announcement.Content,
                IsRead = announcement.IsRead,
                CreatedTime = announcement.CreatedTime,
            });
        }
    }
}
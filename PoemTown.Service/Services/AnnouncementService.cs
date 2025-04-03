using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.AnnouncementRequests;
using PoemTown.Service.BusinessModels.ResponseModels.AnnouncementResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.SignalR;
using PoemTown.Service.SignalR.IReceiveClients;
using PoemTown.Service.SignalR.ReceiveClientModels.AnnouncementClientModels;

namespace PoemTown.Service.Services;

public class AnnouncementService : IAnnouncementService 
{
    private readonly IHubContext<AnnouncementHub, IAnnouncementClient> _hubContext;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    
    public AnnouncementService(IHubContext<AnnouncementHub, IAnnouncementClient> hubContext,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _hubContext = hubContext;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task SendAnnouncementAsync(CreateNewAnnouncementRequest request)
    {
        var connectionId = AnnouncementHub.GetConnectionId(request.UserId);

        Guid announcementId = Guid.NewGuid();
        
        // Send SignalR if user is online
        if (connectionId != string.Empty)
        {
            await _hubContext.Clients.Client(connectionId).ReceiveAnnouncement(new CreateNewAnnouncementClientModel()
            {
                Id = announcementId,
                Title = request.Title,
                Content = request.Content,
                IsRead = request.IsRead
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
            UserId = request.UserId
        });
        
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<GetAnnouncementResponse>> GetUserAnnouncementsAsync(Guid userId)
    {
        var announcements = await _unitOfWork.GetRepository<Announcement>()
            .AsQueryable()
            .Where(a => a.UserId == userId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<GetAnnouncementResponse>>(announcements);
    }
}
using Microsoft.AspNetCore.SignalR;
using PoemTown.Service.Interfaces;
using PoemTown.Service.SignalR;

namespace PoemTown.Service.Services;

public class AnnouncementService : IAnnouncementService 
{
    private readonly IHubContext<AnnouncementHub> _announcementHub;
    
    public AnnouncementService(IHubContext<AnnouncementHub> announcementHub)
    {
        _announcementHub = announcementHub;
    }
    
    /*public async Task SendAnnouncementAsync(string message)
    {
        var connectionId = _announcementHub.
        await _announcementHub.Clients.All.SendAsync("ReceiveAnnouncement", message);
    }*/
}
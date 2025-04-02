using PoemTown.Service.BusinessModels.RequestModels.AnnouncementRequests;

namespace PoemTown.Service.SignalR.IReceiveClients;

public interface IAnnouncementClient
{
    Task ReceiveAnnouncement(CreateNewAnnouncementRequest request);
}
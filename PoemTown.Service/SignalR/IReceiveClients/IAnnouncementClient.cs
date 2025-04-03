using PoemTown.Service.BusinessModels.RequestModels.AnnouncementRequests;
using PoemTown.Service.SignalR.ReceiveClientModels.AnnouncementClientModels;

namespace PoemTown.Service.SignalR.IReceiveClients;

public interface IAnnouncementClient
{
    Task ReceiveAnnouncement(CreateNewAnnouncementClientModel model);
}
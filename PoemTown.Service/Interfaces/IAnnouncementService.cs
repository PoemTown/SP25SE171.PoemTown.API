using PoemTown.Service.BusinessModels.RequestModels.AnnouncementRequests;
using PoemTown.Service.BusinessModels.ResponseModels.AnnouncementResponses;

namespace PoemTown.Service.Interfaces;

public interface IAnnouncementService
{
    Task SendAnnouncementAsync(CreateNewAnnouncementRequest request);
    Task<IEnumerable<GetAnnouncementResponse>> GetUserAnnouncementsAsync(Guid userId);
    Task UpdateAnnouncementToRead(Guid userId, Guid announcementId);
}
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.AnnouncementRequests;
using PoemTown.Service.BusinessModels.ResponseModels.AnnouncementResponses;
using PoemTown.Service.QueryOptions.FilterOptions.AnnouncementFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.AnnouncementSorts;

namespace PoemTown.Service.Interfaces;

public interface IAnnouncementService
{
    Task SendAnnouncementAsync(CreateNewAnnouncementRequest request);
    Task<PaginationResponse<GetAnnouncementResponse>> GetUserAnnouncementsAsync(Guid userId,
        RequestOptionsBase<GetAnnouncementFilterOption, GetAnnouncementSortOption> request);
    Task UpdateAnnouncementToRead(Guid userId, Guid announcementId);
    Task DeleteAnnouncementAsync(Guid userId, Guid announcementId);
    Task DeleteAllUserAnnouncementsAsync(Guid userId);
    Task AdminSendAnnouncementAsync(CreateSystemAnnouncementRequest request);

    Task<PaginationResponse<GetSystemAnnouncementResponse>>
        GetSystemAnnouncements(
            RequestOptionsBase<GetSystemAnnouncementFilterOption, GetSystemAnnouncementSortOption> request);
}
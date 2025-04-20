using PoemTown.Service.BusinessModels.RequestModels.ContentPageRequests;
using PoemTown.Service.BusinessModels.ResponseModels.ContenPageResponses;

namespace PoemTown.Service.Interfaces;

public interface IContentPageService
{
    Task CreateNewContentPage(CreateNewContentPageRequest request);
    Task UpdateContentPage(UpdateContentPageRequest request);
    Task DeleteContentPage(Guid contentPageId);
    Task DeleteContentPagePermanently(Guid contentPageId);
    Task<GetContentPageResponse> GetContentPage(Guid contentPageId);
    Task<IEnumerable<GetContentPageResponse>> GetContentPages();
}
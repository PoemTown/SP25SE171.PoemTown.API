using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PoemHistoryResponses;
using PoemTown.Service.QueryOptions.FilterOptions.PoemHistoryFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemHistorySorts;

namespace PoemTown.Service.Interfaces;

public interface IPoemHistoryService
{
    Task<PaginationResponse<GetPoemHistoryResponse>> GetPoemHistories
        (Guid poemId, RequestOptionsBase<GetPoemHistoryFilterOption, GetPoemHistorySortOptions> request);

    Task DeletePoemHistories(IEnumerable<Guid> poemHistoryIds);
    Task DeletePoemHistory(Guid poemHistoryId);
    Task DeletePoemHistoryPermanent(Guid poemHistoryId);
    Task DeletePoemHistoriesPermanent(IEnumerable<Guid> poemHistoryIds);
    Task<GetPoemHistoryDetailResponse> GetPoemHistoryDetail(Guid poemHistoryId);
}
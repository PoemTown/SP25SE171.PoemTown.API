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
}
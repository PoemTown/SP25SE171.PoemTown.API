using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemSorts;

namespace PoemTown.Service.Interfaces;

public interface IPoemService
{
    Task CreateNewPoem(Guid userId, CreateNewPoemRequest request);

    Task<PaginationResponse<GetPoemResponse>> GetMyPoems
        (Guid userId, RequestOptionsBase<GetMyPoemFilterOption, GetMyPoemSortOption> request);

    Task UpdatePoem(Guid userId, UpdatePoemRequest request);

    Task DeletePoem(Guid poemId);
    Task DeletePoemPermanent(Guid poemId);
}
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.QueryOptions.FilterOptions.CollectionFilters;
using PoemTown.Service.QueryOptions.FilterOptions.TargetMarkFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.CollectionSorts;
using PoemTown.Service.QueryOptions.SortOptions.TargetMarkSorts;

namespace PoemTown.Service.Interfaces;

public interface ITargetMarkService
{
    Task TargetMarkPoem(Guid poemId, Guid userId);
    Task UnTargetMarkPoem(Guid poemId, Guid userId);
    Task TargetMarkCollection(Guid collectionId, Guid userId);
    Task UnTargetMarkCollection(Guid collectionId, Guid userId);
    Task<PaginationResponse<GetPoemInTargetMarkResponse>> GetPoemInTargetMark
        (Guid userId, RequestOptionsBase<GetPoemInTargetMarkFilterOption, GetPoemInTargetMarkSortOption> request);

    Task<PaginationResponse<GetCollectionInTargetMarkResponse>> GetCollectionInTargetMark(
        Guid userId, RequestOptionsBase<GetCollectionInTargetMarkFilterOption, GetCollectionInTargetMarkSortOption> request);
}
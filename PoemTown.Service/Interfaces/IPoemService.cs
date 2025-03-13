using Microsoft.AspNetCore.Http;
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
    Task<PaginationResponse<GetPoemInCollectionResponse>> GetPoemsInCollection
       (Guid userId, Guid collectionId, RequestOptionsBase<GetMyPoemFilterOption, GetMyPoemSortOption> request);
    Task UpdatePoem(Guid userId, UpdatePoemRequest request);

    Task DeletePoem(Guid poemId);
    Task DeletePoemPermanent(Guid poemId);
    Task<GetPoemDetailResponse> 
        GetPoemDetail(Guid userId, Guid poemId, RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request);
    Task<PaginationResponse<GetPostedPoemResponse>> 
        GetPostedPoems(Guid? userId, RequestOptionsBase<GetPoemsFilterOption, GetPoemsSortOption> request);
    Task<PaginationResponse<GetPostedPoemResponse>>
        GetTrendingPoems(Guid? userId, RequestOptionsBase<GetPoemsFilterOption, GetPoemsSortOption> request);

    Task<string> UploadPoemImage(Guid userId, IFormFile file);
    Task EnableSellingPoem(Guid userId, EnableSellingPoemRequest request);
    Task PurchasePoemCopyRight(Guid userId, Guid poemId);
    Task CreatePoemInCommunity(Guid userId, CreateNewPoemRequest request);
    Task DeletePoemInCommunity(Guid userId, Guid poemId);
}
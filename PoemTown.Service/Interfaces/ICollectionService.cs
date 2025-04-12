using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.CollectionRequest;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.QueryOptions.FilterOptions.CollectionFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.CollectionSorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Interfaces
{
    public interface ICollectionService
    {
        Task CreateCollection(Guid userId, CreateCollectionRequest request, string role);
        Task CreateCollectionCommunity(Guid userId, CreateCollectionRequest request);

        Task UpdateCollection(UpdateCollectionRequest request);
        Task<PaginationResponse<GetCollectionResponse>> GetCollections(Guid userId, RequestOptionsBase<CollectionFilterOption, CollectionSortOptions> request);
        Task<PaginationResponse<GetCollectionResponse>> GetCollectionsCommunity(RequestOptionsBase<CollectionFilterOption, CollectionSortOptions> request);
        Task DeleteCollection(Guid userId, Guid collectionId, byte[] rowVersion);
        Task DeleteCollectionPermanent(Guid collectionId);
        Task AddPoemToCollection(Guid poemId, Guid collectionId);
        Task<PaginationResponse<GetCollectionResponse>>GetTrendingCollections(Guid? userId, RequestOptionsBase<CollectionFilterOption, CollectionSortOptions> request);
        Task<GetCollectionResponse> GetCollectionDetail(Guid collectionId, Guid? userId);
        Task<string> UploadCollectionImage(Guid userId, IFormFile file);

        Task<PaginationResponse<GetUserCollectionResponse>>
            GetUserCollections(Guid? userId, string userName, RequestOptionsBase<CollectionFilterOption, CollectionSortOptions> request);
        Task<PaginationResponse<GetCollectionResponse>> GetUserCollections(Guid userId, RequestOptionsBase<CollectionFilterOption, CollectionSortOptions> request);

        Task<PaginationResponse<GetCollectionResponse>>
            GetAllCollections(Guid? userId, RequestOptionsBase<CollectionFilterOption, CollectionSortOptions> request);

        Task CreatePoetSampleCollection(Guid poetSampleId, CreateCollectionRequest request);

        Task<PaginationResponse<GetPoetSampleCollectionResponse>> GetPoetSampleCollection(
            Guid poetSampleId, RequestOptionsBase<CollectionFilterOption, CollectionSortOptions> request);

        Task UpdatePoetSampleCollection(Guid poetSampleId, UpdateCollectionRequest request);
        Task DeletePoetSampleCollection(Guid poetSampleId, Guid collectionId);
    }
}

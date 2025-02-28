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
        Task UpdateCollection(UpdateCollectionRequest request);
        Task<PaginationResponse<GetCollectionResponse>> GetCollections(Guid userId, RequestOptionsBase<CollectionFilterOption, CollectionSortOptions> request);
        Task DeleteCollection(Guid collectionId);
        Task DeleteCollectionPermanent(Guid collectionId);
        Task AddPoemToCollection(Guid poemId, Guid collectionId);
        Task<PaginationResponse<GetCollectionResponse>>
            GetTrendingCollections(Guid? userId, RequestOptionsBase<CollectionFilterOption, CollectionSortOptions> request);
        Task<GetCollectionResponse> GetCollectionDetail(Guid collectionId, Guid userId);
        Task<string> UploadProfileImage(Guid userId, IFormFile file);
    }
}

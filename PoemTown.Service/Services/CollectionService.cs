using AutoMapper;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.CollectionRequest;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.CollectionFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.CollectionSorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CollectionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateCollection(Guid userId, CreateCollectionRequest request)
        {
            Collection collection = _mapper.Map<Collection>(request);
            collection.TotalChapter = 0;
            collection.IsDefault = false;
            collection.UserId = userId;
            await _unitOfWork.GetRepository<Collection>().InsertAsync(collection);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateCollection(UpdateCollectionRequest request)
        {
            Collection collection = await _unitOfWork.GetRepository<Collection>().FindAsync(a => a.Id == request.Id) ?? throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
            _mapper.Map(request, collection);
            _unitOfWork.GetRepository<Collection>().Update(collection);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginationResponse<GetCollectionResponse>> GetCollections(Guid userId, RequestOptionsBase<CollectionFilterOption, CollectionSortOptions> request)
        {
            var collectionQuery = _unitOfWork.GetRepository<Collection>().AsQueryable();
            collectionQuery.Where(a => a.UserId == userId);
            if (request.IsDelete == true)
            {
                collectionQuery = collectionQuery.Where(p => p.DeletedTime != null);
            }
            else
            {
                collectionQuery = collectionQuery.Where(p => p.DeletedTime == null);
            }
            // Apply filter
            if (request.FilterOptions != null)
            {
                if (!String.IsNullOrWhiteSpace(request.FilterOptions.CollectionName))
                {
                    collectionQuery = collectionQuery.Where(p =>
                        p.CollectionName!.Contains(request.FilterOptions.CollectionName.ToLower()));
                }
            }

            // Apply sort
            switch (request.SortOptions)
            {
                case CollectionSortOptions.CreatedTimeAscending:
                    collectionQuery = collectionQuery.OrderBy(p => p.CreatedTime);
                    break;
                case CollectionSortOptions.CreatedTimeDescending:
                    collectionQuery = collectionQuery.OrderByDescending(p => p.CreatedTime);
                    break;
                default:
                    collectionQuery = collectionQuery.OrderByDescending(p => p.CreatedTime);
                    break;
            }

            var queryPaging = await _unitOfWork.GetRepository<Collection>()
            .GetPagination(collectionQuery, request.PageNumber, request.PageSize);

            var collections = _mapper.Map<IList<GetCollectionResponse>>(queryPaging.Data);

            return new PaginationResponse<GetCollectionResponse>(collections, queryPaging.PageNumber, queryPaging.PageSize,
           queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }

        /*  public async Task<GetCollectionResponse> GetCollectionDetail(Guid collectionId, RequestOptionsBase<CollectionFilterOption, CollectionSortOptions> request)
          {
              var collectionQuery = _unitOfWork.GetRepository<Collection>().AsQueryable();
              collectionQuery.Where(a => a.Id == collectionId);

              switch (request.SortOptions)
              {
                  case CollectionSortOptions.CreatedTimeAscending:
                      collectionQuery = collectionQuery.OrderBy(p => p.CreatedTime);
                      break;
                  case CollectionSortOptions.CreatedTimeDescending:
                      collectionQuery = collectionQuery.OrderByDescending(p => p.CreatedTime);
                      break;
                  default:
                      collectionQuery = collectionQuery.OrderByDescending(p => p.CreatedTime);
                      break;
              }
              return null;
          }*/

        public async Task DeleteCollection(Guid collectionId)
        {
            Collection collection = await _unitOfWork.GetRepository<Collection>()
                .FindAsync(a => a.Id == collectionId) ?? throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
           
            _unitOfWork.GetRepository<Collection>().Delete(collection);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCollectionPermanent(Guid collectionId)
        {
            Collection collection = await _unitOfWork.GetRepository<Collection>()
               .FindAsync(a => a.Id == collectionId) ?? throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");

            if (collection.DeletedTime == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Collection is not yet soft deleted");
            }
            _unitOfWork.GetRepository<Collection>().DeletePermanent(collection);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddPoemToCollection(Guid poemId, Guid collectionId)
        {
            Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(a => a.Id == poemId);
            if (poem == null)
            {
                throw new CoreException(StatusCodes.Status404NotFound, "Poem not found");
            }
            Collection collection = await _unitOfWork.GetRepository<Collection>().FindAsync(a => a.Id == collectionId) ?? throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
            if (poem.CollectionId == collectionId)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Poem is already in this collection");
            }
            collection.TotalChapter += 1;
            collection.Poems.Add(poem);
            poem.CollectionId = collectionId;
            _unitOfWork.GetRepository<Poem>().Update(poem);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}



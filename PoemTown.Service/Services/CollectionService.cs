using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
using PoemTown.Repository.Enums.TargetMarks;
using PoemTown.Service.BusinessModels.ResponseModels.TargetMarkResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PoemTown.Repository.Utils;
using PoemTown.Service.ThirdParties.Models.AwsS3;
using PoemTown.Service.ThirdParties.Interfaces;
using PoemTown.Repository.Enums.Accounts;

namespace PoemTown.Service.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAwsS3Service _awsS3Service;
        public CollectionService(IUnitOfWork unitOfWork, IMapper mapper, IAwsS3Service awsS3Service)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _awsS3Service = awsS3Service;
        }

        public async Task CreateCollection(Guid userId, CreateCollectionRequest request, string role)
        {
            Collection collection = _mapper.Map<Collection>(request);
            if (role == "ADMIN" || role == "MOD")
            {
                collection.IsCommunity = true;
            }
            collection.IsDefault = false;
            collection.UserId = userId;
            await _unitOfWork.GetRepository<Collection>().InsertAsync(collection);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateCollection(UpdateCollectionRequest request)
        {
            try
            {
                Collection? collection = await _unitOfWork.GetRepository<Collection>().FindAsync(a => a.Id == request.Id);
                if(collection == null)
                {
                    throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
                }
                if (request.RowVersion != null && !collection.RowVersion.SequenceEqual(request.RowVersion))
                {
                    throw new CoreException(StatusCodes.Status409Conflict, "Collection has been modified by another user. Please refresh and try again.");
                }
                _mapper.Map(request, collection);
                _unitOfWork.GetRepository<Collection>().Update(collection);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new CoreException(StatusCodes.Status409Conflict, "Collection has been modified by another user. Please refresh and try again.");

            }

        }

        public async Task<PaginationResponse<GetCollectionResponse>> GetCollections(Guid userId,
            RequestOptionsBase<CollectionFilterOption, CollectionSortOptions> request)
        {
            var collectionQuery = _unitOfWork.GetRepository<Collection>().AsQueryable();
            collectionQuery = collectionQuery.Where(a => a.UserId == userId );
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


            IList<GetCollectionResponse> collections = new List<GetCollectionResponse>();
            foreach (var collection in queryPaging.Data)
            {
                var collectionEntity =
                    await _unitOfWork.GetRepository<Collection>().FindAsync(p => p.Id == collection.Id);
                if (collectionEntity == null)
                {
                    throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
                }

                collections.Add(_mapper.Map<GetCollectionResponse>(collectionEntity));
                // Assign author to poem by adding into the last element of the list
                collections.Last().User = _mapper.Map<GetBasicUserInformationResponse>(collectionEntity.User);

                collections.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
                (collection.TargetMarks!.FirstOrDefault(tm =>
                    tm.MarkByUserId == userId && tm.CollectionId == collectionEntity.Id &&
                    tm.Type == TargetMarkType.Collection));
            }


            return new PaginationResponse<GetCollectionResponse>(collections, queryPaging.PageNumber,
                queryPaging.PageSize,
                queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }


        public async Task DeleteCollection(Guid collectionId, byte[] rowVersion)
        {
            try
            {
                Collection? collection = await _unitOfWork.GetRepository<Collection>().FindAsync(a => a.Id == collectionId);
                if (collection == null)
                {
                    throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
                }
                if (!collection.RowVersion.SequenceEqual(rowVersion))
                {
                    throw new CoreException(StatusCodes.Status409Conflict,
                        "Collection has been modified by another user. Please refresh and try again.");
                }
                _unitOfWork.GetRepository<Collection>().Delete(collection);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) 
            {
                throw new CoreException(StatusCodes.Status409Conflict, "Collection has been modified by another user. Please refresh and try again.");
            }
            
        }

        public async Task DeleteCollectionPermanent(Guid collectionId)
        {
            Collection? collection = await _unitOfWork.GetRepository<Collection>()
                .FindAsync(a => a.Id == collectionId);
            if (collection == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
            }

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

            Collection? collection = await _unitOfWork.GetRepository<Collection>().FindAsync(a => a.Id == collectionId);
            if (collection == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
            }

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


        public async Task<GetCollectionResponse> GetCollectionDetail(Guid collectionId, Guid userId)
        {
            Collection? collection = await _unitOfWork.GetRepository<Collection>().FindAsync(c => c.Id == collectionId);
            if (collection == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "collection not found");
            }
            var collectionDetail = _mapper.Map<GetCollectionResponse>(collection);
            collectionDetail.User = _mapper.Map<GetBasicUserInformationResponse>(collection?.User);
            collectionDetail.TargetMark = _mapper.Map<GetTargetMarkResponse>(collection.TargetMarks!.FirstOrDefault(tm =>
                    tm.MarkByUserId == userId && tm.CollectionId == collectionDetail.Id &&
                    tm.Type == TargetMarkType.Collection));
            return collectionDetail;
        }

        public async Task<PaginationResponse<GetCollectionResponse>>
            GetTrendingCollections(Guid? userId,
                RequestOptionsBase<CollectionFilterOption, CollectionSortOptions> request)
        {
            var collectionQuery = _unitOfWork.GetRepository<Collection>().AsQueryable();

            // Soft delete filter for collections
            collectionQuery = request.IsDelete == true
                ? collectionQuery.Where(c => c.DeletedTime != null)
                : collectionQuery.Where(c => c.DeletedTime == null);

            // Apply collection name filter
            if (request.FilterOptions != null &&
                !string.IsNullOrWhiteSpace(request.FilterOptions.CollectionName))
            {
                collectionQuery = collectionQuery.Where(c =>
                    c.CollectionName.Contains(request.FilterOptions.CollectionName));
            }

            collectionQuery = collectionQuery.Where(c => c.IsCommunity == false);

            // Calculate trending score
            collectionQuery = collectionQuery
                .Select(c => new
                {
                    Collection = c,
                    TotalLikes = c.Poems
                        .Where(p => p.DeletedTime == null) // Only non-deleted poems
                        .SelectMany(p => p.Likes) // Flatten likes
                        .Count(l => l.DeletedTime == null), // Count non-deleted likes
                    TotalComments = c.Poems
                        .Where(p => p.DeletedTime == null) // Only non-deleted poems
                        .SelectMany(p => p.Comments) // Flatten comments
                        .Count(c => c.DeletedTime == null), // Count non-deleted comments
                    HoursSinceCreation = EF.Functions.DateDiffHour(c.CreatedTime, DateTime.UtcNow)
                })
                .OrderByDescending(x =>
                    (x.TotalLikes + 0.2 * x.TotalComments) / (x.HoursSinceCreation + 2)
                )
                .Select(x => x.Collection);

            var queryPaging = await _unitOfWork.GetRepository<Collection>()
                .GetPagination(collectionQuery, request.PageNumber, request.PageSize);

            //var collections = _mapper.Map<IList<GetCollectionResponse>>(queryPaging.Data);
            IList<GetCollectionResponse> collections = new List<GetCollectionResponse>();
            foreach (var collection in queryPaging.Data)
            {
                var collectionEntity =
                    await _unitOfWork.GetRepository<Collection>().FindAsync(p => p.Id == collection.Id);
                if (collectionEntity == null)
                {
                    throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
                }

                collections.Add(_mapper.Map<GetCollectionResponse>(collectionEntity));
                // Assign author to poem by adding into the last element of the list
                collections.Last().User = _mapper.Map<GetBasicUserInformationResponse>(collectionEntity.User);

                collections.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
                (collection.TargetMarks!.FirstOrDefault(tm =>
                    tm.MarkByUserId == userId && tm.CollectionId == collectionEntity.Id &&
                    tm.Type == TargetMarkType.Collection));
            }

            return new PaginationResponse<GetCollectionResponse>(
                collections,
                queryPaging.PageNumber,
                queryPaging.PageSize,
                queryPaging.TotalRecords,
                queryPaging.CurrentPageRecords
            );
        }

        public async Task<string> UploadProfileImage(Guid userId, IFormFile file)
        {
            var user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);
            if (user == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
            }
            ImageHelper.ValidateImage(file);
            //format file name to avoid duplicate file name with userId, unixTimeStamp
            var fileName = $"collections/{StringHelper.CapitalizeString(userId.ToString())}";
            UploadImageToAwsS3Model s3Model = new UploadImageToAwsS3Model()
            {
                File = file,
                Height = 260,
                Width = 140,
                Quality = 80,
                FolderName = fileName
            };
            return await _awsS3Service.UploadImageToAwsS3Async(s3Model);
        }

        public async Task<PaginationResponse<GetCollectionResponse>> GetCollectionsCommunity(
           RequestOptionsBase<CollectionFilterOption, CollectionSortOptions> request)
        {
            var collectionQuery = _unitOfWork.GetRepository<Collection>().AsQueryable();
            collectionQuery = collectionQuery.Where(a => a.IsCommunity == true);
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


            IList<GetCollectionResponse> collections = new List<GetCollectionResponse>();
            foreach (var collection in queryPaging.Data)
            {
                var collectionEntity =
                    await _unitOfWork.GetRepository<Collection>().FindAsync(p => p.Id == collection.Id);
                if (collectionEntity == null)
                {
                    throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
                }

                collections.Add(_mapper.Map<GetCollectionResponse>(collectionEntity));
                // Assign author to poem by adding into the last element of the list
                collections.Last().User = _mapper.Map<GetBasicUserInformationResponse>(collectionEntity.User);

                collections.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
                (collection.TargetMarks!.FirstOrDefault(tm =>
                    tm.MarkByUserId == collection.UserId && tm.CollectionId == collectionEntity.Id &&
                    tm.Type == TargetMarkType.Collection));
            }


            return new PaginationResponse<GetCollectionResponse>(collections, queryPaging.PageNumber,
                queryPaging.PageSize,
                queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }


    }
}
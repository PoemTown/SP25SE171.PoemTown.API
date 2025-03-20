using AutoMapper;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.TargetMarks;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.LikeResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.TargetMarkResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.CollectionFilters;
using PoemTown.Service.QueryOptions.FilterOptions.TargetMarkFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.CollectionSorts;
using PoemTown.Service.QueryOptions.SortOptions.TargetMarkSorts;

namespace PoemTown.Service.Services;

public class TargetMarkService : ITargetMarkService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public TargetMarkService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task TargetMarkPoem(Guid poemId, Guid userId)
    {
        // Check if poem exists
        Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poemId);
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }
        
        // Check if user already marked the poem
        TargetMark? targetMark = await _unitOfWork.GetRepository<TargetMark>()
            .FindAsync(p => p.PoemId == poemId && p.MarkByUserId == userId);

        if (targetMark != null)
        {
            return;
        }

        targetMark = new TargetMark()
        {
            PoemId = poemId,
            MarkByUserId = userId,
            Type = TargetMarkType.Poem,
        };
        
        await _unitOfWork.GetRepository<TargetMark>().InsertAsync(targetMark);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task TargetMarkCollection(Guid collectionId, Guid userId)
    {
        // Check if collection exists
        Collection? collection = await _unitOfWork.GetRepository<Collection>().FindAsync(p => p.Id == collectionId);
        if (collection == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
        }
        
        // Check if user already marked the collection
        TargetMark? targetMark = await _unitOfWork.GetRepository<TargetMark>()
            .FindAsync(p => p.CollectionId == collectionId && p.MarkByUserId == userId);

        if (targetMark != null)
        {
            return;
        }

        targetMark = new TargetMark()
        {
            CollectionId = collectionId,
            MarkByUserId = userId,
            Type = TargetMarkType.Collection,
        };
        
        await _unitOfWork.GetRepository<TargetMark>().InsertAsync(targetMark);
        await _unitOfWork.SaveChangesAsync();
    }
    
    
    public async Task UnTargetMarkPoem(Guid poemId, Guid userId)
    {
        // Check if poem exists
        Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poemId);
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }
        
        // Check if user has not marked the poem
        TargetMark? targetMark = await _unitOfWork.GetRepository<TargetMark>()
            .FindAsync(p => p.PoemId == poemId && p.MarkByUserId == userId);

        if (targetMark == null)
        {
            return;
        }

        _unitOfWork.GetRepository<TargetMark>().DeletePermanent(targetMark);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task UnTargetMarkCollection(Guid collectionId, Guid userId)
    {
        // Check if collection exists
        Collection? collection = await _unitOfWork.GetRepository<Collection>().FindAsync(p => p.Id == collectionId);
        if (collection == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
        }
        
        // Check if user has not marked the collection
        TargetMark? targetMark = await _unitOfWork.GetRepository<TargetMark>()
            .FindAsync(p => p.CollectionId == collectionId && p.MarkByUserId == userId);

        if (targetMark == null)
        {
            return;
        }

        _unitOfWork.GetRepository<TargetMark>().DeletePermanent(targetMark);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<PaginationResponse<GetPoemInTargetMarkResponse>> GetPoemInTargetMark
        (Guid userId, RequestOptionsBase<GetPoemInTargetMarkFilterOption, GetPoemInTargetMarkSortOption> request)
    {
        var targetMarkQuery = _unitOfWork.GetRepository<TargetMark>().AsQueryable();
        // Get poems that user has marked   
        targetMarkQuery = targetMarkQuery.Where(p => p.MarkByUserId == userId && p.Type == TargetMarkType.Poem);
        
        // Filter options
        if(request.FilterOptions != null)
        {
        }
        
        // Sort options
        targetMarkQuery = request.SortOptions switch
        {
            GetPoemInTargetMarkSortOption.CreatedTimeAscending => targetMarkQuery.OrderBy(p => p.CreatedTime),
            GetPoemInTargetMarkSortOption.CreatedTimeDescending => targetMarkQuery.OrderByDescending(p => p.CreatedTime),
            _ => targetMarkQuery.OrderByDescending(p => p.CreatedTime)
        };
        
        // Paging
        var queryPaging = await _unitOfWork.GetRepository<TargetMark>()
            .GetPagination(targetMarkQuery, request.PageNumber, request.PageSize);
        
        // Assign poems (in queryPaging.Data) to Poem list
        IList<GetPoemInTargetMarkResponse> poems = new List<GetPoemInTargetMarkResponse>();
        foreach (var poem in queryPaging.Data)
        {
            var poemEntity = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poem.PoemId);
            if (poemEntity == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
            }
            poems.Add(_mapper.Map<GetPoemInTargetMarkResponse>(poemEntity));
            // Assign author to poem by adding into the last element of the list
            poems.Last().User = _mapper.Map<GetBasicUserInformationResponse>(poemEntity.Collection!.User);
            poems.Last().Like = _mapper.Map<GetLikeResponse>(poemEntity.Likes!.FirstOrDefault(l => l.UserId == userId && l.PoemId == poemEntity.Id));
            poems.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
                (poemEntity.TargetMarks!.FirstOrDefault(tm => tm.MarkByUserId == userId && tm.PoemId == poemEntity.Id && tm.Type == TargetMarkType.Poem));
        }
        
        return new PaginationResponse<GetPoemInTargetMarkResponse>(poems, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }
    
    public async Task<PaginationResponse<GetCollectionInTargetMarkResponse>> GetCollectionInTargetMark(
        Guid userId, RequestOptionsBase<GetCollectionInTargetMarkFilterOption, GetCollectionInTargetMarkSortOption> request)
    {
        var targetMarkQuery = _unitOfWork.GetRepository<TargetMark>().AsQueryable();
        // Get collections that user has marked   
        targetMarkQuery = targetMarkQuery.Where(p => p.MarkByUserId == userId && p.Type == TargetMarkType.Collection);
        
        // Filter options
        if(request.FilterOptions != null)
        {
        }
        
        // Sort options
        targetMarkQuery = request.SortOptions switch
        {
            GetCollectionInTargetMarkSortOption.CreatedTimeAscending => targetMarkQuery.OrderBy(p => p.CreatedTime),
            GetCollectionInTargetMarkSortOption.CreatedTimeDescending => targetMarkQuery.OrderByDescending(p => p.CreatedTime),
            _ => targetMarkQuery.OrderByDescending(p => p.CreatedTime)
        };
        
        // Paging
        var queryPaging = await _unitOfWork.GetRepository<TargetMark>()
            .GetPagination(targetMarkQuery, request.PageNumber, request.PageSize);
        
        // Assign collections (in queryPaging.Data) to Collection list
        List<GetCollectionInTargetMarkResponse> collections = new List<GetCollectionInTargetMarkResponse>();
        foreach (var collection in queryPaging.Data)
        {
            var collectionEntity = await _unitOfWork.GetRepository<Collection>().FindAsync(p => p.Id == collection.CollectionId);
            if (collectionEntity == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
            }
            collections.Add(_mapper.Map<GetCollectionInTargetMarkResponse>(collectionEntity));
            // Assign author to collection by adding into the last element of the list
            if (collectionEntity.IsCommunity == false) { 
                collections.Last().User = _mapper.Map<GetBasicUserInformationResponse>(collectionEntity.User);
            }
            collections.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
                (collectionEntity.TargetMarks!.FirstOrDefault(tm => tm.MarkByUserId == userId && tm.CollectionId == collectionEntity.Id && tm.Type == TargetMarkType.Collection));
        }
        
        return new PaginationResponse<GetCollectionInTargetMarkResponse>(collections, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }
}
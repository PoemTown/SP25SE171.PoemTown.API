using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemSorts;

namespace PoemTown.Service.Services;

public class PoemService : IPoemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public PoemService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task CreateNewPoem(Guid userId, CreateNewPoemRequest request) 
    {
        // Mapping request to entity
        Poem poem = _mapper.Map<CreateNewPoemRequest, Poem>(request);

        // Check if source copy right exist
        if (request.SourceCopyRight != null)
        {
            User? sourceCopyRight = await _unitOfWork.GetRepository<User>()
                .FindAsync(p => p.Id == request.SourceCopyRight);
            if(sourceCopyRight == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Source copy right not found");
            }
        }

        Collection? collection = null;
        
        // If collectionId is not null then check if the collection exist
        if (request.CollectionId != null)
        {
            collection = await _unitOfWork.GetRepository<Collection>()
                .FindAsync(p => p.Id == request.CollectionId);
            if(collection == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
            }
        }
        
        // Check if user has any collection, if not then create default collection, finally assign that poem to default collection
        bool existCollections = await _unitOfWork.GetRepository<Collection>()
            .AsQueryable()
            .AnyAsync(p => p.UserId == userId);
        if (!existCollections)
        {
            collection = new Collection
            {
                UserId = userId,
                CollectionName = "Bộ sưu tập mặc định",
                CollectionDescription = "Bộ sưu tập được khởi tạo mặc định bởi hệ thống",
                IsDefault = true
            };
            await _unitOfWork.GetRepository<Collection>().InsertAsync(collection);
        }
        
        poem.Collection = collection;
        poem.UserId = userId;
        // Add poem to database
        await _unitOfWork.GetRepository<Poem>().InsertAsync(poem);
        
        // Save changes
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PaginationResponse<GetPoemResponse>> GetMyPoems
        (Guid userId, RequestOptionsBase<GetMyPoemFilterOption, GetMyPoemSortOption> request)
    {
        var poemQuery = _unitOfWork.GetRepository<Poem>().AsQueryable();
        
        poemQuery.Where(p => p.UserId == userId);
        
        // Apply filter
        if (request.FilterOptions != null)
        {
            if (!String.IsNullOrWhiteSpace(request.FilterOptions.Title))
            {
                poemQuery = poemQuery.Where(p => p.Title!.Contains(request.FilterOptions.Title, StringComparison.OrdinalIgnoreCase));
            }
            
            if (!String.IsNullOrWhiteSpace(request.FilterOptions.ChapterName))
            {
                poemQuery = poemQuery.Where(p => p.ChapterName!.Contains(request.FilterOptions.ChapterName, StringComparison.OrdinalIgnoreCase));
            }
            
            if(request.FilterOptions.Type != null)
            {
                poemQuery = poemQuery.Where(p => p.Type == request.FilterOptions.Type);
            }

            if(request.FilterOptions.Status != null)
            {
                poemQuery = poemQuery.Where(p => p.Status == request.FilterOptions.Status);
            }
            
            if(request.FilterOptions.CollectionId != null)
            {
                poemQuery = poemQuery.Where(p => p.CollectionId == request.FilterOptions.CollectionId);
            }
        }
        
        // Apply sort
        switch (request.SortOptions)
        {
            case GetMyPoemSortOption.ViewCountAscending:
                poemQuery = poemQuery.OrderBy(p => p.ViewCount);
                break;
            case GetMyPoemSortOption.ViewCountDescending:
                poemQuery = poemQuery.OrderByDescending(p => p.ViewCount);
                break;
            case GetMyPoemSortOption.LikeCountAscending:
                poemQuery = poemQuery.OrderBy(p => p.LikeCount);
                break;
            case GetMyPoemSortOption.LikeCountDescending:
                poemQuery = poemQuery.OrderByDescending(p => p.LikeCount);
                break;
            case GetMyPoemSortOption.CommentCountAscending:
                poemQuery = poemQuery.OrderBy(p => p.CommentCount);
                break;
            case GetMyPoemSortOption.CommentCountDescending:
                poemQuery = poemQuery.OrderByDescending(p => p.CommentCount);
                break;
            case GetMyPoemSortOption.TypeAscending:
                poemQuery = poemQuery.OrderBy(p => p.Type);
                break;
            case GetMyPoemSortOption.TypeDescending:
                poemQuery = poemQuery.OrderByDescending(p => p.Type);
                break;
            default:
                poemQuery = poemQuery.OrderByDescending(p => p.CreatedTime);
                break;
        }
        
        var queryPaging = await _unitOfWork.GetRepository<Poem>()
                    .GetPagination(poemQuery, request.PageNumber, request.PageSize);

        var poems = _mapper.Map<IList<GetPoemResponse>>(queryPaging.Data);

        foreach (var poem in poems)
        {
            var sourceCopyRight = poem.SourceCopyRight.Id;
            var user = await _unitOfWork.GetRepository<User>()
                .FindAsync(p => p.Id == sourceCopyRight);
            
        }
        
        return new PaginationResponse<GetPoemResponse>(poems, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }
}
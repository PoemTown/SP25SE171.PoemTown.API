using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;
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

        // Check if source copy right exist, if not then throw exception else assign source copy right to poem
        if (request.SourceCopyRightId != null)
        {
            UserPoemRecordFile? sourceCopyRight = await _unitOfWork.GetRepository<UserPoemRecordFile>()
                .FindAsync(p => p.Id == request.SourceCopyRightId && p.UserId == userId);
            if (sourceCopyRight == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Source copy right not found");
            }

            poem.SourceCopyRightId = sourceCopyRight.Id;
        }

        Collection? collection = null;

        // Check if user has any collection, if not then create default collection, first assign that poem to default collection
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
        else
        {
            collection = await _unitOfWork.GetRepository<Collection>()
                .FindAsync(p => p.UserId == userId && p.IsDefault == true);
        }

        // If collectionId is not null then check if the collection exist
        if (request.CollectionId != null)
        {
            collection = await _unitOfWork.GetRepository<Collection>()
                .FindAsync(p => p.Id == request.CollectionId && p.UserId == userId);
            if (collection == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
            }
        }

        poem.Collection = collection;
        
        // Add poem to database
        await _unitOfWork.GetRepository<Poem>().InsertAsync(poem);

        // Initiate poem history
        PoemHistory poemHistory = new PoemHistory();
        _mapper.Map(poem, poemHistory);
        poemHistory.PoemId = poem.Id;
        poemHistory.Version = 1;
        await _unitOfWork.GetRepository<PoemHistory>().InsertAsync(poemHistory);
        
        // Save changes
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetPoemDetailResponse> 
        GetPoemDetail(Guid userId, Guid poemId, RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
    {
        Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poemId);
        
        if(poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }
        
        // Check if user own this poem
        if (poem.Collection!.UserId != userId)
        {
            throw new CoreException(StatusCodes.Status403Forbidden, "User does not own this poem");
        }
        
        var poemDetail = _mapper.Map<GetPoemDetailResponse>(poem);
        
        if (poem.RecordFiles != null && poem.RecordFiles.Count <= 0)
        {
            return poemDetail;
        }
        
        // Get record files of poem with filter, sort and paging
        var recordFilesQuery = _unitOfWork.GetRepository<RecordFile>()
            .AsQueryable();

        recordFilesQuery = recordFilesQuery.Where(p => p.PoemId == poemId && p.Poem!.Collection!.UserId == userId);
        
        switch (request.SortOptions)
        {
            case GetPoemRecordFileDetailSortOption.CreatedTimeAscending:
                recordFilesQuery = recordFilesQuery.OrderBy(p => p.CreatedTime);
                break;
            case GetPoemRecordFileDetailSortOption.CreatedTimeDescending:
                recordFilesQuery = recordFilesQuery.OrderByDescending(p => p.CreatedTime);
                break;
            default:
                recordFilesQuery = recordFilesQuery.OrderByDescending(p => p.CreatedTime);
                break;
        }
        
        var queryPaging = await _unitOfWork.GetRepository<RecordFile>()
            .GetPagination(recordFilesQuery, request.PageNumber, request.PageSize);

        poemDetail.RecordFiles = new PaginationResponse<GetRecordFileResponse>
        (
            _mapper.Map<IList<GetRecordFileResponse>>(queryPaging.Data),
            queryPaging.PageNumber,
            queryPaging.PageSize,
            queryPaging.TotalRecords,
            queryPaging.CurrentPageRecords
        );

        return poemDetail;
    }
    
    public async Task<PaginationResponse<GetPoemResponse>> GetMyPoems
        (Guid userId, RequestOptionsBase<GetMyPoemFilterOption, GetMyPoemSortOption> request)
    {
        var poemQuery = _unitOfWork.GetRepository<Poem>().AsQueryable();

        poemQuery = poemQuery.Where(p => p.Collection!.UserId == userId);
        
        if(request.IsDelete == true)
        {
            poemQuery = poemQuery.Where(p => p.DeletedTime != null);
        }
        else
        {
            poemQuery = poemQuery.Where(p => p.DeletedTime == null);
        }
        // Apply filter
        if (request.FilterOptions != null)
        {
            if (!String.IsNullOrWhiteSpace(request.FilterOptions.Title))
            {
                poemQuery = poemQuery.Where(p =>
                    p.Title!.Contains(request.FilterOptions.Title, StringComparison.OrdinalIgnoreCase));
            }

            if (!String.IsNullOrWhiteSpace(request.FilterOptions.ChapterName))
            {
                poemQuery = poemQuery.Where(p =>
                    p.ChapterName!.Contains(request.FilterOptions.ChapterName, StringComparison.OrdinalIgnoreCase));
            }

            if (request.FilterOptions.Type != null)
            {
                poemQuery = poemQuery.Where(p => p.Type == request.FilterOptions.Type);
            }

            if (request.FilterOptions.Status != null)
            {
                poemQuery = poemQuery.Where(p => p.Status == request.FilterOptions.Status);
            }

            if (request.FilterOptions.CollectionId != null)
            {
                poemQuery = poemQuery.Where(p => p.CollectionId == request.FilterOptions.CollectionId);
            }
        }

        // Apply sort
        switch (request.SortOptions)
        {
            case GetMyPoemSortOption.LikeCountAscending:
                poemQuery = poemQuery.OrderBy(p => p.Likes!.Count(l => l.PoemId == p.Id));
                break;
            case GetMyPoemSortOption.LikeCountDescending:
                poemQuery = poemQuery.OrderByDescending(p => p.Likes!.Count(l => l.PoemId == p.Id));
                break;
            case GetMyPoemSortOption.CommentCountAscending:
                poemQuery = poemQuery.OrderBy(p => p.Comments!.Count(c => c.PoemId == p.Id));
                break;
            case GetMyPoemSortOption.CommentCountDescending:
                poemQuery = poemQuery.OrderByDescending(p => p.Comments!.Count(c => c.PoemId == p.Id));
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

        /*
        foreach (var poem in poems)
        {
            var copyRights = queryPaging.Data
                .SelectMany(p => p.UserPoems!.Where(up => up.UserId == userId && up.PoemId == poem.Id))
                .ToList();

            poem.CopyRights = _mapper.Map<IList<GetUserPoemResponse>>(copyRights);
        }*/
        return new PaginationResponse<GetPoemResponse>(poems, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }

    public async Task UpdatePoem(Guid userId, UpdatePoemRequest request)
    {
        // (Nếu có) Trong trường hợp chỉnh sửa status của poem thành POSTED thì không cần lưu bản history (Chưa hoàn thiện)
        
        // Find poem by id
        Poem? poem = await _unitOfWork.GetRepository<Poem>()
            .FindAsync(p => p.Id == request.Id);

        // If poem not found then throw exception
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }

        // If poem is posted then throw exception
        if (poem.Status != PoemStatus.Draft)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem is not a draft, cannot update");
        }

        // Check if source copy right exist, if not then throw exception else assign source copy right to poem
        if (request.SourceCopyRightId != null)
        {
            UserPoemRecordFile? sourceCopyRight = await _unitOfWork.GetRepository<UserPoemRecordFile>()
                .FindAsync(p => p.Id == request.SourceCopyRightId && p.UserId == userId);
            if (sourceCopyRight == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Source copy right not found");
            }
        }

        // If collectionId is not null then check if the collection exist
        Collection? collection = await _unitOfWork.GetRepository<Collection>()
            .FindAsync(p => p.Id == request.CollectionId && p.UserId == userId);
        if (collection == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
        }

        _mapper.Map(request, poem);
        _unitOfWork.GetRepository<Poem>().Update(poem);

        // Mapping poem to poem history and create versioning for poem
        PoemHistory poemHistory = new PoemHistory();
        _mapper.Map(poem, poemHistory);
        poemHistory.PoemId = poem.Id;
        poemHistory.Version = await _unitOfWork.GetRepository<PoemHistory>()
            .AsQueryable()
            .CountAsync(p => request.Id == p.PoemId) + 1;
        await _unitOfWork.GetRepository<PoemHistory>().InsertAsync(poemHistory);
        
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeletePoem(Guid poemId)
    {
        // Find poem by id
        Poem? poem = await _unitOfWork.GetRepository<Poem>()
            .FindAsync(p => p.Id == poemId);

        // If poem not found then throw exception
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }

        // If poem is posted then throw exception
        if (poem.Status != PoemStatus.Draft)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem is not a draft, cannot delete");
        }
        
        _unitOfWork.GetRepository<Poem>().Delete(poem);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeletePoemPermanent(Guid poemId)
    {
        // Find poem by id
        Poem? poem = await _unitOfWork.GetRepository<Poem>()
            .FindAsync(p => p.Id == poemId);

        // If poem not found then throw exception
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        } 
        
        // If poem is not soft deleted then throw exception
        if (poem.DeletedTime == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem is not yet soft deleted");
        }

        // If poem is posted then throw exception
        if (poem.Status != PoemStatus.Draft)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem is not a draft, cannot delete");
        }
        
        _unitOfWork.GetRepository<Poem>().DeletePermanent(poem);
        await _unitOfWork.SaveChangesAsync();
    }
}
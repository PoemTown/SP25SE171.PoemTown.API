using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Repository.Enums.TargetMarks;
using PoemTown.Repository.Enums.UserPoems;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.ResponseModels.LikeResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;
using PoemTown.Service.BusinessModels.ResponseModels.TargetMarkResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemSorts;
using PoemTown.Service.ThirdParties.Interfaces;
using PoemTown.Service.ThirdParties.Models.AwsS3;

namespace PoemTown.Service.Services;

public class PoemService : IPoemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAwsS3Service _awsS3Service;
    public PoemService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IAwsS3Service awsS3Service)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _awsS3Service = awsS3Service;
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
        
        // Assign author to poem
        poemDetail.User = _mapper.Map<GetBasicUserInformationResponse>(poem.Collection!.User);
        
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

            /*if (request.FilterOptions.CollectionId != null)
            {
                poemQuery = poemQuery.Where(p => p.CollectionId == request.FilterOptions.CollectionId);
            }*/
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

        //var poems = _mapper.Map<IList<GetPoemResponse>>(queryPaging.Data);

        /*
        foreach (var poem in poems)
        {
            var copyRights = queryPaging.Data
                .SelectMany(p => p.UserPoems!.Where(up => up.UserId == userId && up.PoemId == poem.Id))
                .ToList();

            poem.CopyRights = _mapper.Map<IList<GetUserPoemResponse>>(copyRights);
        }*/
        
        // Assign poems (in queryPaging.Data) to Poem list
        IList<GetPoemResponse> poems = new List<GetPoemResponse>();
        foreach (var poem in queryPaging.Data)
        {
            var poemEntity = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poem.Id);
            if (poemEntity == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
            }
            poems.Add(_mapper.Map<GetPoemResponse>(poemEntity));
            // Assign author to poem by adding into the last element of the list
            poems.Last().User = _mapper.Map<GetBasicUserInformationResponse>(poemEntity.Collection!.User);
            
            poems.Last().Like = _mapper.Map<GetLikeResponse>(poemEntity.Likes!.FirstOrDefault(l => l.UserId == userId && l.PoemId == poemEntity.Id));
            poems.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
                (poemEntity.TargetMarks!.FirstOrDefault(tm => tm.MarkByUserId == userId && tm.PoemId == poemEntity.Id && tm.Type == TargetMarkType.Poem));
        }
        
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
            .Where(p => request.Id == p.PoemId && p.DeletedTime == null)
            .MaxAsync(p => p.Version) + 1;
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

    public async Task<PaginationResponse<GetPostedPoemResponse>>
        GetPostedPoems(Guid? userId, RequestOptionsBase<GetPoemsFilterOption, GetPoemsSortOption> request)
    {
        var poemQuery = _unitOfWork.GetRepository<Poem>().AsQueryable();

        poemQuery = poemQuery.Where(p => p.Status == PoemStatus.Posted);

        if (request.IsDelete == true)
        {
            poemQuery = poemQuery.Where(p => p.DeletedTime != null);
        }
        else
        {
            poemQuery = poemQuery.Where(p => p.DeletedTime == null);
        }

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

            if (request.FilterOptions.Type != null)
            {
                poemQuery = poemQuery.Where(p => p.Type == request.FilterOptions.Type);
            }

            /*if (request.FilterOptions.CollectionId != null)
            {
                poemQuery = poemQuery.Where(p => p.CollectionId == request.FilterOptions.CollectionId);
            }   */

            if(request.FilterOptions.AudioStatus != null)
            {
                if (request.FilterOptions.AudioStatus == PoemAudio.HaveAudio)
                {
                    poemQuery = poemQuery.Where(p => p.RecordFiles.Any());
                }
                if (request.FilterOptions.AudioStatus == PoemAudio.NoAudio)
                {
                    poemQuery = poemQuery.Where(p => !p.RecordFiles.Any());
                }
            }
        }

        //apply sort
        switch(request.SortOptions)
        {
            case GetPoemsSortOption.LikeCountAscending:
                poemQuery = poemQuery.OrderBy(p => p.Likes!.Count(l => l.PoemId == p.Id));
                break;
            case GetPoemsSortOption.LikeCountDescending:
                poemQuery = poemQuery.OrderByDescending(p => p.Likes!.Count(l => l.PoemId == p.Id));
                break;
            case GetPoemsSortOption.CommentCountAscending:
                poemQuery = poemQuery.OrderBy(p => p.Comments!.Count(c => c.PoemId == p.Id));
                break;
            case GetPoemsSortOption.CommentCountDescending:
                poemQuery = poemQuery.OrderByDescending(p => p.Comments!.Count(c => c.PoemId == p.Id));
                break;
            case GetPoemsSortOption.TypeAscending:
                poemQuery = poemQuery.OrderBy(p => p.Type);
                break;
            case GetPoemsSortOption.TypeDescending:
                poemQuery = poemQuery.OrderByDescending(p => p.Type);
                break;
            default:
                poemQuery = poemQuery.OrderByDescending(p => p.CreatedTime);
                break;
        }

        var queryPaging = await _unitOfWork.GetRepository<Poem>()
            .GetPagination(poemQuery, request.PageNumber, request.PageSize);
        
        //var poems = _mapper.Map<IList<GetPoemResponse>>(queryPaging.Data);

        IList<GetPostedPoemResponse> poems = new List<GetPostedPoemResponse>();
        foreach (var poem in queryPaging.Data)
        {
            var poemEntity = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poem.Id);
            if (poemEntity == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
            }
            poems.Add(_mapper.Map<GetPostedPoemResponse>(poemEntity));
            // Assign author to poem by adding into the last element of the list
            poems.Last().User = _mapper.Map<GetBasicUserInformationResponse>(poemEntity.Collection!.User);
            
            // Assign like to poem by adding into the last element of the list
            poems.Last().Like = _mapper.Map<GetLikeResponse>(poemEntity.Likes!.FirstOrDefault(l => l.UserId == userId && l.PoemId == poemEntity.Id));
            poems.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
                (poemEntity.TargetMarks!.FirstOrDefault(tm => tm.MarkByUserId == userId && tm.PoemId == poemEntity.Id && tm.Type == TargetMarkType.Poem));
        }
        
        return new PaginationResponse<GetPostedPoemResponse>(poems, queryPaging.PageNumber, queryPaging.PageSize, 
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);

    }

    public async Task<PaginationResponse<GetPoemInCollectionResponse>> GetPoemsInCollection
         (Guid userId, Guid collectionId, RequestOptionsBase<GetMyPoemFilterOption, GetMyPoemSortOption> request)
    {
        Collection? collection = await _unitOfWork.GetRepository<Collection>().FindAsync(a => a.Id == collectionId);
        if(collection == null)
        {
            throw new CoreException(StatusCodes.Status404NotFound, "Collection not found");
        }
        /*if (collection.DeletedTime != null)
        {
            throw new CoreException(StatusCodes.Status404NotFound, "Collection is deleted");
        }*/
        var poemQuery = _unitOfWork.GetRepository<Poem>().AsQueryable();

        poemQuery = poemQuery.Where(p => p.Collection!.Id == collectionId);

        if (request.IsDelete == true)
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

            /*if (request.FilterOptions.CollectionId != null)
            {
                poemQuery = poemQuery.Where(p => p.CollectionId == request.FilterOptions.CollectionId);
            }*/
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

        //var poems = _mapper.Map<IList<GetPoemResponse>>(queryPaging.Data);
        
        IList<GetPoemInCollectionResponse> poems = new List<GetPoemInCollectionResponse>();
        foreach (var poem in queryPaging.Data)
        {
            var poemEntity = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poem.Id);
            if (poemEntity == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
            }
            poems.Add(_mapper.Map<GetPoemInCollectionResponse>(poemEntity));
            // Assign author to poem by adding into the last element of the list
            poems.Last().User = _mapper.Map<GetBasicUserInformationResponse>(poemEntity.Collection!.User);
            poems.Last().Like = _mapper.Map<GetLikeResponse>(poemEntity.Likes!.FirstOrDefault(l => l.UserId == userId && l.PoemId == poemEntity.Id));
            poems.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
                (poemEntity.TargetMarks!.FirstOrDefault(tm => tm.MarkByUserId == userId && tm.PoemId == poemEntity.Id && tm.Type == TargetMarkType.Poem));
        }
        
        return new PaginationResponse<GetPoemInCollectionResponse>(poems, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }

    public async Task<PaginationResponse<GetPostedPoemResponse>>
        GetTrendingPoems(Guid? userId, RequestOptionsBase<GetPoemsFilterOption, GetPoemsSortOption> request)
    {
        var poemQuery = _unitOfWork.GetRepository<Poem>().AsQueryable();

        poemQuery = poemQuery.Where(p => p.Status == PoemStatus.Posted);

        poemQuery = poemQuery
         .Select(p => new
         {
             Poem = p,
             Score = (p.Likes!.Count * 1.0 + p.Comments!.Count * 0.2) /
                     ((EF.Functions.DateDiffHour(p.CreatedTime, DateTime.UtcNow) + 2) *
                      Math.Sqrt(EF.Functions.DateDiffHour(p.CreatedTime, DateTime.UtcNow) + 2))
         })
         .OrderByDescending(x => x.Score)
         .Select(x => x.Poem);

        if (request.IsDelete == true)
        {
            poemQuery = poemQuery.Where(p => p.DeletedTime != null);
        }
        else
        {
            poemQuery = poemQuery.Where(p => p.DeletedTime == null);
        }

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

            if (request.FilterOptions.Type != null)
            {
                poemQuery = poemQuery.Where(p => p.Type == request.FilterOptions.Type);
            }

            /*
            if (request.FilterOptions.CollectionId != null)
            {
                poemQuery = poemQuery.Where(p => p.CollectionId == request.FilterOptions.CollectionId);
            }
            */

            if (request.FilterOptions.AudioStatus != null)
            {
                if (request.FilterOptions.AudioStatus == PoemAudio.HaveAudio)
                {
                    poemQuery = poemQuery.Where(p => p.RecordFiles.Any());
                }
                if (request.FilterOptions.AudioStatus == PoemAudio.NoAudio)
                {
                    poemQuery = poemQuery.Where(p => !p.RecordFiles.Any());
                }
            }
        }

        //apply sort
        switch (request.SortOptions)
        {
            case GetPoemsSortOption.LikeCountAscending:
                poemQuery = poemQuery.OrderBy(p => p.Likes!.Count(l => l.PoemId == p.Id));
                break;
            case GetPoemsSortOption.LikeCountDescending:
                poemQuery = poemQuery.OrderByDescending(p => p.Likes!.Count(l => l.PoemId == p.Id));
                break;
            case GetPoemsSortOption.CommentCountAscending:
                poemQuery = poemQuery.OrderBy(p => p.Comments!.Count(c => c.PoemId == p.Id));
                break;
            case GetPoemsSortOption.CommentCountDescending:
                poemQuery = poemQuery.OrderByDescending(p => p.Comments!.Count(c => c.PoemId == p.Id));
                break;
            case GetPoemsSortOption.TypeAscending:
                poemQuery = poemQuery.OrderBy(p => p.Type);
                break;
            case GetPoemsSortOption.TypeDescending:
                poemQuery = poemQuery.OrderByDescending(p => p.Type);
                break;
            default:
                break;
        }

        var queryPaging = await _unitOfWork.GetRepository<Poem>()
            .GetPagination(poemQuery, request.PageNumber, request.PageSize);

        //var poems = _mapper.Map<IList<GetPoemResponse>>(queryPaging.Data);

        IList<GetPostedPoemResponse> poems = new List<GetPostedPoemResponse>();
        foreach (var poem in queryPaging.Data)
        {
            var poemEntity = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poem.Id);
            if (poemEntity == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
            }
            poems.Add(_mapper.Map<GetPostedPoemResponse>(poemEntity));
            // Assign author to poem by adding into the last element of the list
            poems.Last().User = _mapper.Map<GetBasicUserInformationResponse>(poemEntity.Collection!.User);
            
            // Assign like to poem by adding into the last element of the list
            poems.Last().Like = _mapper.Map<GetLikeResponse>(poemEntity.Likes!.FirstOrDefault(l => l.UserId == userId && l.PoemId == poemEntity.Id));
            poems.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
                (poemEntity.TargetMarks!.FirstOrDefault(tm => tm.MarkByUserId == userId && tm.PoemId == poemEntity.Id && tm.Type == TargetMarkType.Poem));
        }
        
        return new PaginationResponse<GetPostedPoemResponse>(poems, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }
    
    public async Task<string> UploadPoemImage(Guid userId, IFormFile file)
    {
        var user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);
        // If user not found then throw exception
        if (user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }
        ImageHelper.ValidateImage(file);

        // Upload image to AWS S3
        var fileName = $"poems/{StringHelper.CapitalizeString(userId.ToString())}";
        UploadImageToAwsS3Model s3Model = new UploadImageToAwsS3Model()
        {
            File = file,
            FolderName = fileName
        };
        return await _awsS3Service.UploadImageToAwsS3Async(s3Model);
    }
    
    public async Task EnableSellingPoem(Guid userId, EnableSellingPoemRequest request)
    {
        // Find poem by id
        Poem? poem = await _unitOfWork.GetRepository<Poem>()
            .FindAsync(p => p.Id == request.PoemId);

        // If poem not found then throw exception
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }

        // Check if user own this poem
        if(poem.Collection!.UserId != userId)
        {
            throw new CoreException(StatusCodes.Status403Forbidden, "User does not own this poem");
        }
        
        // If poem is not posted then throw exception
        if (poem.Status != PoemStatus.Posted)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem is not posted, cannot enable selling");
        }

        // If poem is already selling then throw exception
        if (poem.Price > 0)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem is already selling");
        }

        poem.Price = request.Price;
        poem.IsPublic = false;
        
        _unitOfWork.GetRepository<Poem>().Update(poem);

        UserPoemRecordFile? userPoemRecordFile = new UserPoemRecordFile()
        {
            UserId = userId,
            PoemId = poem.Id,
            Type = UserPoemType.CopyRightHolder,
        };
        
        await _unitOfWork.GetRepository<UserPoemRecordFile>().InsertAsync(userPoemRecordFile);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task PurchasePoemCopyRight(Guid userId, Guid poemId)
    {
        // Find poem by id
        Poem? poem = await _unitOfWork.GetRepository<Poem>()
            .FindAsync(p => p.Id == poemId);

        // If poem not found then throw exception
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }

        // If poem is public then throw exception
        if (poem.IsPublic == true)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem is public, cannot purchase");
        }

        // Find user by id
        UserPoemRecordFile? userPoemRecordFile = await _unitOfWork.GetRepository<UserPoemRecordFile>()
            .FindAsync(p => p.UserId == userId && p.PoemId == poemId);

        // If user already purchased this poem then throw exception
        if (userPoemRecordFile != null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User already purchased this poem");
        }

        // Create new user poem record file for buyer with 2 years valid copy right
        userPoemRecordFile = new UserPoemRecordFile
        {
            UserId = userId,
            PoemId = poemId,
            Type = UserPoemType.Buyer,
            CopyRightValidFrom = DateTimeHelper.SystemTimeNow.DateTime,
            CopyRightValidTo = DateTimeHelper.SystemTimeNow.AddYears(2).DateTime
        };
        
        await _unitOfWork.GetRepository<UserPoemRecordFile>().InsertAsync(userPoemRecordFile);
        await _unitOfWork.SaveChangesAsync();
    }
}
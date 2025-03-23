using AutoMapper;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Orders;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Repository.Enums.SaleVersions;
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
using PoemTown.Service.Events.OrderEvents;
using PoemTown.Service.Events.PoemEvents;
using PoemTown.Service.Interfaces;
using PoemTown.Service.PlagiarismDetector.Interfaces;
using PoemTown.Service.PlagiarismDetector.PDModels;
using PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemSorts;
using PoemTown.Service.ThirdParties.Interfaces;
using PoemTown.Service.ThirdParties.Models.AwsS3;
using PoemTown.Service.ThirdParties.Models.TheHiveAi;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PoemTown.Service.Services;

public class PoemService : IPoemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAwsS3Service _awsS3Service;
    private readonly IOpenAIService _openAiService;
    private readonly ITheHiveAiService _theHiveAiService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IQDrantService _qDrantService;

    public PoemService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IAwsS3Service awsS3Service,
        IOpenAIService openAiService,
        ITheHiveAiService theHiveAiService,
        IPublishEndpoint publishEndpoint,
        IQDrantService qDrantService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _awsS3Service = awsS3Service;
        _openAiService = openAiService;
        _theHiveAiService = theHiveAiService;
        _publishEndpoint = publishEndpoint;
        _qDrantService = qDrantService;
    }

    public async Task CreateNewPoem(Guid userId, CreateNewPoemRequest request)
    {
        // Mapping request to entity
        Poem poem = _mapper.Map<CreateNewPoemRequest, Poem>(request);

        // Check if source copy right exist, if not then throw exception else assign source copy right to poem
        if (request.SourceCopyRightId != null)
        {
            UsageRight? sourceCopyRight = await _unitOfWork.GetRepository<UsageRight>()
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
        poem.UserId = userId;

        // Add poem to database
        await _unitOfWork.GetRepository<Poem>().InsertAsync(poem);

        // Initiate poem history
        PoemHistory poemHistory = new PoemHistory();
        _mapper.Map(poem, poemHistory);
        poemHistory.PoemId = poem.Id;
        poemHistory.Version = 1;
        await _unitOfWork.GetRepository<PoemHistory>().InsertAsync(poemHistory);

/**/ /*// When poem is posted, assign user to poem as copy right holder
        if (request.Status == PoemStatus.Posted)
        {
            await _unitOfWork.GetRepository<UsageRight>().InsertAsync(new UsageRight()
            {
                Type = UserPoemType.CopyRightHolder,
                UserId = userId,
                PoemId = poem.Id,
            });
        }*/

        // Check if poem is posted
        if (request.Status == PoemStatus.Posted)
        {
            bool isExist = await _unitOfWork.GetRepository<SaleVersion>()
                .AsQueryable()
                .AnyAsync(p => p.PoemId == poem.Id);
            
            // Check if sale version exist, if not then create default sale version for poem
            if (!isExist)
            {
                // Create default sale version for poem
                await _unitOfWork.GetRepository<SaleVersion>().InsertAsync(new SaleVersion
                {
                    PoemId = poem.Id,
                    CommissionPercentage = 0,
                    DurationTime = 100,
                    IsInUse = true,
                    Status = SaleVersionStatus.Default,
                    Price = 0,
                });
            }
        }
        
        // Save changes
        await _unitOfWork.SaveChangesAsync();

        // Publish event to store poem embedding
        await _publishEndpoint.Publish<CheckPoemPlagiarismEvent>(new
        {
            PoemId = poem.Id,
            UserId = userId,
            PoemContent = poem.Content
        });
    }

    public async Task CreatePoemInCommunity(Guid userId, CreateNewPoemRequest request)
    {
        // Mapping request to entity
        Poem poem = _mapper.Map<CreateNewPoemRequest, Poem>(request);

        // Check if source copy right exist, if not then throw exception else assign source copy right to poem
        if (request.SourceCopyRightId != null)
        {
            UsageRight? sourceCopyRight = await _unitOfWork.GetRepository<UsageRight>()
                .FindAsync(p => p.Id == request.SourceCopyRightId && p.UserId == userId);
            if (sourceCopyRight == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Source copy right not found");
            }

            poem.SourceCopyRightId = sourceCopyRight.Id;
        }

        Collection? collection = null;

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
        // Assign user to poem
        /*UsageRight userPoemRecord = new UsageRight
        {
            PoemId = poem.Id,
            UserId = userId,
            Type = UserPoemType.CopyRightHolder,
        };
        await _unitOfWork.GetRepository<UsageRight>().InsertAsync(userPoemRecord);*/
        
        // Create new sale version as FREE for poem because this is community poem
        if (request.Status == PoemStatus.Posted)
        {
            bool isExist = await _unitOfWork.GetRepository<SaleVersion>()
                .AsQueryable()
                .AnyAsync(p => p.PoemId == poem.Id);
            
            // Check if sale version exist, if not then create default sale version for poem
            if (!isExist)
            {
                // Create default sale version for poem
                await _unitOfWork.GetRepository<SaleVersion>().InsertAsync(new SaleVersion
                {
                    PoemId = poem.Id,
                    CommissionPercentage = 0,
                    DurationTime = 100,
                    IsInUse = true,
                    Status = SaleVersionStatus.Free,
                    Price = 0,
                });
            }
        }
        
        // Save changes
        await _unitOfWork.SaveChangesAsync();
    }


    public async Task<GetPoemDetailResponse>
        GetPoemDetail(Guid userId, Guid poemId,
            RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
    {
        Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poemId);

        if (poem == null)
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

            poems.Last().Like =
                _mapper.Map<GetLikeResponse>(
                    poemEntity.Likes!.FirstOrDefault(l => l.UserId == userId && l.PoemId == poemEntity.Id));
            poems.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
            (poemEntity.TargetMarks!.FirstOrDefault(tm =>
                tm.MarkByUserId == userId && tm.PoemId == poemEntity.Id && tm.Type == TargetMarkType.Poem));
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
            UsageRight? sourceCopyRight = await _unitOfWork.GetRepository<UsageRight>()
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

        /*// When poem is posted, assign user to poem as copy right holder
        if (request.Status == PoemStatus.Posted)
        {
            await _unitOfWork.GetRepository<UsageRight>().InsertAsync(new UsageRight()
            {
                Type = UserPoemType.CopyRightHolder,
                PoemId = poem.Id,
                UserId = userId,
            });
        }*/

        if (request.Status == PoemStatus.Posted)
        {
            bool isExist = await _unitOfWork.GetRepository<SaleVersion>()
                .AsQueryable()
                .AnyAsync(p => p.PoemId == poem.Id);
            
            // Check if sale version exist, if not then create default sale version for poem
            if (!isExist)
            {
                // Create default sale version for poem
                await _unitOfWork.GetRepository<SaleVersion>().InsertAsync(new SaleVersion
                {
                    PoemId = poem.Id,
                    CommissionPercentage = 0,
                    DurationTime = 100,
                    IsInUse = true,
                    Status = SaleVersionStatus.Default,
                    Price = 0,
                });
            }
        }
        
        await _unitOfWork.SaveChangesAsync();

        // Publish event to store poem embedding
        await _publishEndpoint.Publish<CheckPoemPlagiarismEvent>(new
        {
            PoemId = poem.Id,
            UserId = userId,
            PoemContent = poem.Content
        });
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


    public async Task DeletePoemInCommunity(Guid userId, Guid poemId)
    {
        // Find poem by id
        Poem? poem = await _unitOfWork.GetRepository<Poem>()
            .FindAsync(p => p.Id == poemId);
        // If poem not found then throw exception
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }

        // Check if poem is not yours
        if (poem.UserId != userId)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "This poem is not yours");
        }

        /*UsageRight? userPoemRecordFile = await _unitOfWork.GetRepository<UsageRight>()
            .FindAsync(r => r.PoemId == poemId && r.UserId == userId && r.Type == UserPoemType.CopyRightHolder);
        //Check if record file is not yours
        if (userPoemRecordFile == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "This poem is not yours");
        }*/
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

            /*if (request.FilterOptions.CollectionId != null)
            {
                poemQuery = poemQuery.Where(p => p.CollectionId == request.FilterOptions.CollectionId);
            }   */

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
            poems.Last().Like =
                _mapper.Map<GetLikeResponse>(
                    poemEntity.Likes!.FirstOrDefault(l => l.UserId == userId && l.PoemId == poemEntity.Id));
            poems.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
            (poemEntity.TargetMarks!.FirstOrDefault(tm =>
                tm.MarkByUserId == userId && tm.PoemId == poemEntity.Id && tm.Type == TargetMarkType.Poem));
        }

        return new PaginationResponse<GetPostedPoemResponse>(poems, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }

    public async Task<PaginationResponse<GetPoemInCollectionResponse>> GetPoemsInCollection
        (Guid userId, Guid collectionId, RequestOptionsBase<GetMyPoemFilterOption, GetMyPoemSortOption> request)
    {
        Collection? collection = await _unitOfWork.GetRepository<Collection>().FindAsync(a => a.Id == collectionId);
        if (collection == null)
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
            poems.Last().Like =
                _mapper.Map<GetLikeResponse>(
                    poemEntity.Likes!.FirstOrDefault(l => l.UserId == userId && l.PoemId == poemEntity.Id));
            poems.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
            (poemEntity.TargetMarks!.FirstOrDefault(tm =>
                tm.MarkByUserId == userId && tm.PoemId == poemEntity.Id && tm.Type == TargetMarkType.Poem));
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
            poems.Last().Like =
                _mapper.Map<GetLikeResponse>(
                    poemEntity.Likes!.FirstOrDefault(l => l.UserId == userId && l.PoemId == poemEntity.Id));
            poems.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
            (poemEntity.TargetMarks!.FirstOrDefault(tm =>
                tm.MarkByUserId == userId && tm.PoemId == poemEntity.Id && tm.Type == TargetMarkType.Poem));
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

    public async Task SellingSaleVersionPoem(Guid userId, SellingSaleVersionPoemRequest request)
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
        if (poem.UserId != userId)
        {
            throw new CoreException(StatusCodes.Status403Forbidden, "User does not own this poem");
        }

        // If poem is not posted then throw exception
        if (poem.Status != PoemStatus.Posted)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem is not posted, cannot enable selling");
        }

        // Disable selling for community poem
        if (poem.Collection is { IsCommunity: true })
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem is community poem, cannot enable selling");
        }
        
        IList<SaleVersion> saleVersions = await _unitOfWork.GetRepository<SaleVersion>()
            .AsQueryable()
            .Where(p => p.PoemId == poem.Id && p.Poem.UserId == userId)
            .ToListAsync();

        // If poem has sale versions then set all to not in sale
        if (saleVersions.Count > 0)
        {
            foreach (var sv in saleVersions)
            {
                sv.Status = SaleVersionStatus.NotInSale;
                sv.IsInUse = false;
            }

            _unitOfWork.GetRepository<SaleVersion>().UpdateRange(saleVersions);
        }

        // Finally, create new sale version for poem with status is in sale
        SaleVersion saleVersion = new SaleVersion()
        {
            PoemId = poem.Id,
            Price = request.Price,
            DurationTime = request.DurationTime,
            CommissionPercentage = request.CommissionPercentage,
            IsInUse = true,
            Status = SaleVersionStatus.InSale,
        };

        await _unitOfWork.GetRepository<SaleVersion>().InsertAsync(saleVersion);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task FreeSaleVersionPoem(Guid userId, Guid poemId)
    {
        // Find poem by id
        Poem? poem = await _unitOfWork.GetRepository<Poem>()
            .FindAsync(p => p.Id == poemId);

        // If poem not found then throw exception
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }

        // Check if user own this poem
        if (poem.UserId != userId)
        {
            throw new CoreException(StatusCodes.Status403Forbidden, "User does not own this poem");
        }

        // If poem is not posted then throw exception
        if (poem.Status != PoemStatus.Posted)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem is not posted, cannot enable selling");
        }

        // Check if poem already has free sale version
        bool anyFreeSaleVersion = await _unitOfWork.GetRepository<SaleVersion>()
            .AsQueryable()
            .AnyAsync(p => p.PoemId == poem.Id && p.Status == SaleVersionStatus.Free);
        if(anyFreeSaleVersion)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem already has free sale version");
        }
        
        IList<SaleVersion> saleVersions = await _unitOfWork.GetRepository<SaleVersion>()
            .AsQueryable()
            .Where(p => p.PoemId == poem.Id && p.Poem.UserId == userId)
            .ToListAsync();

        // If poem has sale versions then set all to not in sale
        if (saleVersions.Count > 0)
        {
            foreach (var sv in saleVersions)
            {
                sv.IsInUse = false;
                sv.Status = SaleVersionStatus.NotInSale;
            }

            _unitOfWork.GetRepository<SaleVersion>().UpdateRange(saleVersions);
        }

        // Finally, create new sale version as free for poem with status is in sale
        SaleVersion saleVersion = new SaleVersion()
        {
            PoemId = poem.Id,
            Price = 0,
            DurationTime = 100,
            CommissionPercentage = 0,
            IsInUse = true,
            Status = SaleVersionStatus.Free,
        };

        await _unitOfWork.GetRepository<SaleVersion>().InsertAsync(saleVersion);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task PurchasePoemCopyRight(Guid userId, Guid poemId)
    {
        // Find sale version of poem by id
        Poem? poem = await _unitOfWork.GetRepository<Poem>()
            .FindAsync(p => p.Id == poemId);

        // If poem not found then throw exception
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }

        // If user is owner of this poem then throw exception
        if(poem.UserId == userId)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User is owner of this poem");
        }
        
        // Get the latest sale version of poem
        SaleVersion? saleVersion = await _unitOfWork.GetRepository<SaleVersion>()
            .FindAsync(p => p.PoemId == poemId && p.Status == SaleVersionStatus.InSale);
        
        if(saleVersion == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem is not for sale");
        }

        // Cannot purchase free poem
        if (saleVersion.Status == SaleVersionStatus.Free)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem is free, cannot purchase");
        }
        /*// If poem is public then throw exception
        if (saleVersion.Poem.IsSellCopyRight == false)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem is not for sell, cannot purchase");
        }*/

        UserEWallet? userEWallet = await _unitOfWork.GetRepository<UserEWallet>()
            .FindAsync(p => p.UserId == userId);

        // If user e-wallet not found then throw exception
        if (userEWallet == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User e-wallet not found");
        }

        // If user e-wallet balance is not enough then throw exception
        if (userEWallet.WalletBalance < saleVersion.Price)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User e-wallet balance is not enough");
        }

        UsageRight? usageRight = await _unitOfWork.GetRepository<UsageRight>()
            .FindAsync(p => p.UserId == userId
                            && p.SaleVersion!.PoemId == poemId
                            && p.Type == UserPoemType.PoemBuyer);
        
        // If user already purchased this poem then throw exception
        if (usageRight != null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest,
                "User already purchased this version sale of poem");
        }

        // Create new user usageRight for buyer with saleVersion.DurationTime years valid copy right
        usageRight = new UsageRight
        {
            UserId = userId,
            Type = UserPoemType.PoemBuyer,
            SaleVersion = saleVersion,
            CopyRightValidFrom = DateTimeHelper.SystemTimeNow.DateTime,
            CopyRightValidTo = DateTimeHelper.SystemTimeNow.AddYears(saleVersion.DurationTime).DateTime
        };

        await _unitOfWork.GetRepository<UsageRight>().InsertAsync(usageRight);

        // Deduct user e-wallet balance
        userEWallet.WalletBalance -= saleVersion.Price;
        
        _unitOfWork.GetRepository<UserEWallet>().Update(userEWallet);

        await _unitOfWork.SaveChangesAsync();

        CreateOrderEvent message = new CreateOrderEvent()
        {
            OrderCode = OrderCodeGenerator.Generate(),
            Amount = saleVersion.Price,
            Type = OrderType.Poems,
            OrderDescription = $"Mua quyền sử dụng bài thơ {saleVersion.Poem.Title}",
            Status = OrderStatus.Paid,
            SaleVersionId = saleVersion.Id,
            PaidDate = DateTimeHelper.SystemTimeNow,
            DiscountAmount = 0,
            UserId = userId
        };
        await _publishEndpoint.Publish(message);
    }

    public async Task<string> PoemAiChatCompletion(PoemAiChatCompletionRequest request)
    {
        var chatCompletionCreateRequest = new ChatCompletionCreateRequest()
        {
            Messages = new List<ChatMessage>()
            {
                new ChatMessage("system", "Bạn là con trí tuệ nhân tạo thơ, giỏi viết những bài thơ tiếng Việt đẹp."),
                new ChatMessage("user", $"Thể thơ: {EnumHelper.GetDescription(request.Type)}"),
                new ChatMessage("user", $"Nội dung thơ: {request.PoemContent}"),
                new ChatMessage("user", $"Câu hỏi: {request.ChatContent}"),
            },
            // Set max token for completion
            MaxTokens = request.MaxToken,

            // Set model for chat completion
            Model = Models.Gpt_4o_mini
        };

        var response = await _openAiService.ChatCompletion.CreateCompletion(chatCompletionCreateRequest);

        // If response is not successful then throw exception
        if (response.Successful == false)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, response.Error?.Message);
        }

        return response.Choices.FirstOrDefault()?.Message.Content ?? "";
    }

    public async Task<string> ConvertPoemTextToImage(ConvertPoemTextToImageRequest request)
    {
        // Create image with OpenAI model DALL-E 3
        ImageCreateRequest imageCreateRequest = new ImageCreateRequest()
        {
            Model = Models.Dall_e_3,
            N = 1,
            Prompt = $"Nội dung thơ: {request.PoemText}\n Câu hỏi: {request.Prompt}",
            Size = EnumHelper.GetDescription(request.ImageSize),
            Style = EnumHelper.GetDescription(request.ImageStyle),
            //Quality = EnumHelper.GetDescription(request.ImageQuality),
            Quality = "standard",
        };
        var response = await _openAiService.Image.CreateImage(imageCreateRequest);

        // If response is not successful then throw exception
        if (response.Successful == false)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, response.Error?.Message);
        }

        return response.Results.FirstOrDefault().Url;
    }

    public async Task<string> TranslatePoemTextVietnameseIntoEnglish(string poemText)
    {
        // Create chat completion request for translation
        ChatCompletionCreateRequest chatCompletionCreateRequest = new ChatCompletionCreateRequest()
        {
            Messages = new List<ChatMessage>()
            {
                new ChatMessage("system",
                    "Bạn là con trí tuệ nhân tạo dịch thơ, giỏi dịch những bài thơ tiếng Việt sang tiếng Anh."),
                new ChatMessage("user", $"Nội dung thơ: {poemText}"),
                new ChatMessage("user", "Câu hỏi: Dịch thơ sang tiếng Anh"),
                new ChatMessage("user", "Lưu ý: Chỉ trả kết là bản dịch từ tiếng Việt sang tiếng Anh")
            },
            MaxTokens = 250,
            Model = Models.Gpt_4o_mini
        };

        var response = await _openAiService.ChatCompletion.CreateCompletion(chatCompletionCreateRequest);

        // If response is not successful then throw exception
        if (response.Successful == false)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, response.Error?.Message);
        }

        return response.Choices.FirstOrDefault()?.Message.Content ?? "";
    }

    public async Task<TheHiveAiResponse> ConvertPoemTextToImageWithTheHiveAiFluxSchnellEnhanced(
        ConvertPoemTextToImageWithTheHiveAiFluxSchnellEnhancedRequest request)
    {
        // Translate poem text from Vietnamese to English
        string translatedPoemText = await TranslatePoemTextVietnameseIntoEnglish(request.PoemText);

        ConvertTextToImageWithFluxSchnellEnhancedModel model = new ConvertTextToImageWithFluxSchnellEnhancedModel()
        {
            Prompt = $"Poem content: {translatedPoemText}",
            NegativePrompt = request.NegativePrompt,
            ImageSize = request.ImageSize,
            NumberInferenceSteps = request.NumberInferenceSteps,
            NumberOfImages = request.NumberOfImages,
            OutPutFormat = request.OutPutFormat,
            OutPutQuality = request.OutPutQuality
        };

        var response = await _theHiveAiService.ConvertTextToImageWithFluxSchnellEnhanced(model);

        return response;
    }

    public async Task<TheHiveAiResponse> ConvertPoemTextToImageWithTheHiveAiSdxlEnhanced(
        ConvertPoemTextToImageWithTheHiveAiSdxlEnhancedRequest request)
    {
        // Translate poem text from Vietnamese to English
        string translatedPoemText = await TranslatePoemTextVietnameseIntoEnglish(request.PoemText);

        ConvertTextToImageWithSdxlEnhancedModel model = new ConvertTextToImageWithSdxlEnhancedModel()
        {
            Prompt = $"Poem content: {translatedPoemText}",
            NegativePrompt = request.NegativePrompt,
            ImageSize = request.ImageSize,
            NumberInferenceSteps = request.NumberInferenceSteps,
            GuidanceScale = request.GuidanceScale,
            NumberOfImages = request.NumberOfImages,
            OutPutFormat = request.OutPutFormat,
            OutPutQuality = request.OutPutQuality
        };

        var response = await _theHiveAiService.ConvertTextToImageWithSdxlEnhanced(model);

        return response;
    }

    public async Task ConvertPoemIntoEmbeddingAndSaveToQdrant(Guid poemId)
    {
        Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poemId);
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }

        await _qDrantService.StorePoemEmbeddingAsync(poemId, poem.Collection!.UserId, poem.Content!);
    }

    public bool IsPoemPlagiarism(double score)
    {
        return score > 0.5;
    }

    public async Task<PoemPlagiarismResponse> CheckPoemPlagiarism(Guid userId, string poemContent)
    {
        // Search similar poem embedding point
        var response = await _qDrantService.SearchSimilarPoemEmbeddingPoint(userId, poemContent);

        double averageScore = response.Results.Select(p => p.Score).Average();

        return new PoemPlagiarismResponse()
        {
            Score = averageScore,
            IsPlagiarism = IsPoemPlagiarism(averageScore)
        };
    }
}
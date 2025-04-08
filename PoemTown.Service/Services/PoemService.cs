using AutoMapper;
using MassTransit;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Announcements;
using PoemTown.Repository.Enums.Orders;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Repository.Enums.SaleVersions;
using PoemTown.Repository.Enums.TargetMarks;
using PoemTown.Repository.Enums.UsageRights;
using PoemTown.Repository.Enums.UserPoems;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.ResponseModels.LikeResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;
using PoemTown.Service.BusinessModels.ResponseModels.TargetMarkResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Consumers.AnnouncementConsumers;
using PoemTown.Service.Events.AnnouncementEvents;
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
            bool isExistSaleVersion = await _unitOfWork.GetRepository<SaleVersion>()
                .AsQueryable()
                .AnyAsync(p => p.PoemId == poem.Id);

            // Check if sale version exist, if not then create default sale version for poem
            if (!isExistSaleVersion)
            {
                SaleVersion saleVersion = new SaleVersion
                {
                    PoemId = poem.Id,
                    CommissionPercentage = 0,
                    DurationTime = 100,
                    IsInUse = true,
                    Status = SaleVersionStatus.Default,
                    Price = 0,
                };
                // Create default sale version for poem
                await _unitOfWork.GetRepository<SaleVersion>().InsertAsync(saleVersion);

                // Create usage right for user as copy right holder
                var now = DateTimeHelper.SystemTimeNow.DateTime;
                await _unitOfWork.GetRepository<UsageRight>().InsertAsync(new UsageRight()
                {
                    UserId = userId,
                    Type = UserPoemType.CopyRightHolder,
                    Status = UsageRightStatus.StillValid,
                    CopyRightValidFrom = now,
                    CopyRightValidTo = now.AddYears(100),
                    SaleVersion = saleVersion,
                });

                // Adjust created time of poem when it is posted
                poem.CreatedTime = DateTimeHelper.SystemTimeNow;
            }

            // Publish event to store poem embedding
            await _publishEndpoint.Publish<CheckPoemPlagiarismEvent>(new
            {
                PoemId = poem.Id,
                UserId = userId,
                PoemContent = poem.Content
            });
        }

        // Save changes
        await _unitOfWork.SaveChangesAsync();

        if (poem.Status == PoemStatus.Posted)
        {
            var announcePoem = await _unitOfWork.GetRepository<Poem>()
                .FindAsync(p => p.Id == poem.Id);
            
            if (announcePoem == null)
            {
                return;
            }
            // Announce user when poem created
            await AnnounceUserWhenPoemCreated(userId, announcePoem);
        }
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
                SaleVersion saleVersion = new SaleVersion
                {
                    PoemId = poem.Id,
                    CommissionPercentage = 0,
                    DurationTime = 100,
                    IsInUse = true,
                    Status = SaleVersionStatus.Free,
                    Price = 0,
                };
                await _unitOfWork.GetRepository<SaleVersion>().InsertAsync(saleVersion);

                // Create usage right for user as copy right holder
                var now = DateTimeHelper.SystemTimeNow.DateTime;
                await _unitOfWork.GetRepository<UsageRight>().InsertAsync(new UsageRight()
                {
                    UserId = userId,
                    Type = UserPoemType.CopyRightHolder,
                    Status = UsageRightStatus.StillValid,
                    CopyRightValidFrom = now,
                    CopyRightValidTo = now.AddYears(100),
                    SaleVersion = saleVersion,
                });

                // Adjust created time of poem when it is posted
                poem.CreatedTime = DateTimeHelper.SystemTimeNow;
            }


            // Publish event to store poem embedding
            await _publishEndpoint.Publish<CheckPoemPlagiarismEvent>(new
            {
                PoemId = poem.Id,
                UserId = userId,
                PoemContent = poem.Content
            });
        }

        // Save changes
        await _unitOfWork.SaveChangesAsync();
        if (poem.Status == PoemStatus.Posted)
        {
            var announcePoem = await _unitOfWork.GetRepository<Poem>()
                .FindAsync(p => p.Id == poem.Id);
            
            if (announcePoem == null)
            {
                return;
            }
            // Announce user when poem created
            await AnnounceUserWhenPoemCreated(userId, announcePoem);
        }
    }


    public async Task<GetPoemDetailResponse>
        GetPoemDetail(Guid? userId, Guid poemId,
            RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
    {
        Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poemId);

        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }


        /*
        // Check if user own this poem
        if (poem.Collection!.UserId != userId)
        {
            throw new CoreException(StatusCodes.Status403Forbidden, "User does not own this poem");
        }*/

        var poemDetail = _mapper.Map<GetPoemDetailResponse>(poem);

        // Assign author to poem
        poemDetail.User = _mapper.Map<GetBasicUserInformationResponse>(poem.User);

        if (userId != null)
        {
            poemDetail.IsMine = false;

            User? user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);
            // Check if user own this poem
            if (user == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
            }

            if (poem.UserId == userId)
            {
                poemDetail.IsMine = true;
            }

            // Check if user is able to upload record file for this poem
            bool isAbleToUploadRecordFile =
                // Allow to upload record file if poem is free
                await _unitOfWork.GetRepository<SaleVersion>()
                    .AsQueryable()
                    .AnyAsync(p => p.IsInUse == true
                                   && p.PoemId == poemId
                                   && p.Status == SaleVersionStatus.Free) ||

                // Allow to upload record file if user is copy right holder
                await _unitOfWork.GetRepository<UsageRight>()
                    .AsQueryable()
                    .AnyAsync(p => p.SaleVersion != null
                                   && p.SaleVersion.PoemId == poemId
                                   && p.UserId == userId
                                   && p.Status == UsageRightStatus.StillValid);

            poemDetail.IsAbleToUploadRecordFile = isAbleToUploadRecordFile;

            poemDetail.Like =
                _mapper.Map<GetLikeResponse>(
                    poem.Likes!.FirstOrDefault(l => l.UserId == userId && l.PoemId == poem.Id));

            poemDetail.TargetMark = _mapper.Map<GetTargetMarkResponse>(
                poem.TargetMarks!.FirstOrDefault(tm =>
                    tm.MarkByUserId == userId && tm.PoemId == poem.Id && tm.Type == TargetMarkType.Poem));

            // Check if user is followed this user
            poemDetail.IsFollowed = poem.User!.FollowedUser!.Any(p => p.FollowUserId == userId);
        }

        if (poem.RecordFiles != null && poem.RecordFiles.Count <= 0)
        {
            return poemDetail;
        }

        // Get record files of poem with filter, sort and paging
        var recordFilesQuery = _unitOfWork.GetRepository<RecordFile>()
            .AsQueryable();

        recordFilesQuery = recordFilesQuery.Where(p => p.PoemId == poemId && p.DeletedTime == null);

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
            poems.Last().User = _mapper.Map<GetBasicUserInformationResponse>(poemEntity.User);

            poems.Last().Like =
                _mapper.Map<GetLikeResponse>(
                    poemEntity.Likes!.FirstOrDefault(l => l.UserId == userId && l.PoemId == poemEntity.Id));
            poems.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
            (poemEntity.TargetMarks!.FirstOrDefault(tm =>
                tm.MarkByUserId == userId && tm.PoemId == poemEntity.Id && tm.Type == TargetMarkType.Poem));

            // Check if user is able to upload record file for this poem
            bool isAbleToUploadRecordFile =
                // Allow to upload record file if poem is free
                await _unitOfWork.GetRepository<SaleVersion>()
                    .AsQueryable()
                    .AnyAsync(p => p.IsInUse == true
                                   && p.PoemId == poemEntity.Id
                                   && p.Status == SaleVersionStatus.Free) ||

                // Allow to upload record file if user is copy right holder
                await _unitOfWork.GetRepository<UsageRight>()
                    .AsQueryable()
                    .AnyAsync(p => p.SaleVersion != null
                                   && p.SaleVersion.PoemId == poemEntity.Id
                                   && p.UserId == userId
                                   && p.Status == UsageRightStatus.StillValid);

            poems.Last().IsAbleToUploadRecordFile = isAbleToUploadRecordFile;
        }

        return new PaginationResponse<GetPoemResponse>(poems, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }

    private async Task AnnounceUserWhenPoemCreated(Guid poetId, Poem poem)
    {
        // --------------Follower-------------- //

        // Get all users who follow this user
        var rawUserIds = await _unitOfWork.GetRepository<Follower>()
            .AsQueryable()
            .Where(p => p.FollowedUserId == poetId && p.FollowUserId != null)
            .Select(p => p.FollowUserId)
            .ToListAsync();

        // Filter out the author of the poem from the list of user ids
        var userIds = rawUserIds.Where(id => id.HasValue && id.Value != poetId)
            .Select(id => id ?? default)
            .ToList();

        var poet = await _unitOfWork.GetRepository<User>()
            .FindAsync(p => p.Id == poetId);
        // Announce to all users who follow this user if authorPoem is not null
        // Send announcement to all users who follow this user
        await _publishEndpoint.Publish(new SendBulkUserAnnouncementEvent
        {
            UserIds = userIds,
            Title = "Có một bài thơ mới được đăng tải",
            Content = $"Bài thơ {poem.Title} của {poet?.UserName} đã được đăng tải",
            Type = AnnouncementType.Poem,
            PoemId = poem.Id
        });

        // --------------Bookmarks-------------- //

        // Get all users who bookmark this poem collection
        var rawBookMarkUserIds = await _unitOfWork.GetRepository<TargetMark>()
            .AsQueryable()
            .Where(p => p.CollectionId == poem.CollectionId && p.MarkByUserId != null)
            .Select(p => p.MarkByUserId)
            .ToListAsync();

        // Filter out the author of the poem from the list of user ids
        var bookMarkUserIds = rawBookMarkUserIds.Where(id => id.HasValue && id.Value != poetId)
            .Select(id => id ?? default)
            .Except(userIds)
            .ToList();

        // Send announcement to all users who bookmark this poem collection
        await _publishEndpoint.Publish(new SendBulkUserAnnouncementEvent
        {
            UserIds = bookMarkUserIds,
            Title = "Bộ sưu tập bạn theo dõi có bài thơ mới được đăng tải",
            Content =
                $"Bộ sưu tập mà bạn đã theo dõi: {poem.Collection!.CollectionName} có bài thơ {poem.Title} đã được đăng tải, hãy ghé xem ngay nhé!",
            Type = AnnouncementType.Poem,
            PoemId = poem.Id
        });
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
                SaleVersion saleVersion = new SaleVersion
                {
                    PoemId = poem.Id,
                    CommissionPercentage = 0,
                    DurationTime = 100,
                    IsInUse = true,
                    Status = SaleVersionStatus.Free,
                    Price = 0,
                };
                await _unitOfWork.GetRepository<SaleVersion>().InsertAsync(saleVersion);

                // Create usage right for user as copy right holder
                var now = DateTimeHelper.SystemTimeNow.DateTime;
                await _unitOfWork.GetRepository<UsageRight>().InsertAsync(new UsageRight()
                {
                    UserId = userId,
                    Type = UserPoemType.CopyRightHolder,
                    Status = UsageRightStatus.StillValid,
                    CopyRightValidFrom = now,
                    CopyRightValidTo = now.AddYears(100),
                    SaleVersion = saleVersion,
                });

                // Adjust created time of poem when it is posted
                poem.CreatedTime = DateTimeHelper.SystemTimeNow;
            }

            // Publish event to store poem embedding
            await _publishEndpoint.Publish<CheckPoemPlagiarismEvent>(new
            {
                PoemId = poem.Id,
                UserId = userId,
                PoemContent = poem.Content
            });
        }

        await _unitOfWork.SaveChangesAsync();
        if (poem.Status == PoemStatus.Posted)
        {
            var announcePoem = await _unitOfWork.GetRepository<Poem>()
                .FindAsync(p => p.Id == poem.Id);
            
            if (announcePoem == null)
            {
                return;
            }
            // Announce user when poem created
            await AnnounceUserWhenPoemCreated(userId, announcePoem);
        }
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
            poems.Last().User = _mapper.Map<GetBasicUserInformationResponse>(poemEntity.User);

            // Assign like to poem by adding into the last element of the list
            poems.Last().Like =
                _mapper.Map<GetLikeResponse>(
                    poemEntity.Likes!.FirstOrDefault(l => l.UserId == userId && l.PoemId == poemEntity.Id));
            poems.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
            (poemEntity.TargetMarks!.FirstOrDefault(tm =>
                tm.MarkByUserId == userId && tm.PoemId == poemEntity.Id && tm.Type == TargetMarkType.Poem));

            // Check if user is able to upload record file for this poem
            bool isAbleToUploadRecordFile =
                // Allow to upload record file if poem is free
                await _unitOfWork.GetRepository<SaleVersion>()
                    .AsQueryable()
                    .AnyAsync(p => p.IsInUse == true
                                   && p.PoemId == poemEntity.Id
                                   && p.Status == SaleVersionStatus.Free) ||

                // Allow to upload record file if user is copy right holder
                await _unitOfWork.GetRepository<UsageRight>()
                    .AsQueryable()
                    .AnyAsync(p => p.SaleVersion != null
                                   && p.SaleVersion.PoemId == poemEntity.Id
                                   && p.UserId == userId
                                   && p.Status == UsageRightStatus.StillValid);

            poems.Last().IsAbleToUploadRecordFile = isAbleToUploadRecordFile;
        }

        return new PaginationResponse<GetPostedPoemResponse>(poems, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }

    public async Task<PaginationResponse<GetPoemInCollectionResponse>> GetPoemsInCollection
        (Guid? userId, Guid collectionId, RequestOptionsBase<GetMyPoemFilterOption, GetMyPoemSortOption> request)
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
            poems.Last().User = _mapper.Map<GetBasicUserInformationResponse>(poemEntity.User);
            poems.Last().Like =
                _mapper.Map<GetLikeResponse>(
                    poemEntity.Likes!.FirstOrDefault(l => l.UserId == userId && l.PoemId == poemEntity.Id));
            poems.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
            (poemEntity.TargetMarks!.FirstOrDefault(tm =>
                tm.MarkByUserId == userId && tm.PoemId == poemEntity.Id && tm.Type == TargetMarkType.Poem));

            // Check if user is able to upload record file for this poem
            bool isAbleToUploadRecordFile =
                // Allow to upload record file if poem is free
                await _unitOfWork.GetRepository<SaleVersion>()
                    .AsQueryable()
                    .AnyAsync(p => p.IsInUse == true
                                   && p.PoemId == poemEntity.Id
                                   && p.Status == SaleVersionStatus.Free) ||

                // Allow to upload record file if user is copy right holder
                await _unitOfWork.GetRepository<UsageRight>()
                    .AsQueryable()
                    .AnyAsync(p => p.SaleVersion != null
                                   && p.SaleVersion.PoemId == poemEntity.Id
                                   && p.UserId == userId
                                   && p.Status == UsageRightStatus.StillValid);

            poems.Last().IsAbleToUploadRecordFile = isAbleToUploadRecordFile;

            if (poemEntity.UserId == userId)
            {
                poems.Last().IsMine = true;
            }
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
            poems.Last().User = _mapper.Map<GetBasicUserInformationResponse>(poemEntity.User);

            // Assign like to poem by adding into the last element of the list
            poems.Last().Like =
                _mapper.Map<GetLikeResponse>(
                    poemEntity.Likes!.FirstOrDefault(l => l.UserId == userId && l.PoemId == poemEntity.Id));
            poems.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
            (poemEntity.TargetMarks!.FirstOrDefault(tm =>
                tm.MarkByUserId == userId && tm.PoemId == poemEntity.Id && tm.Type == TargetMarkType.Poem));

            // Check if user is able to upload record file for this poem
            bool isAbleToUploadRecordFile =
                // Allow to upload record file if poem is free
                await _unitOfWork.GetRepository<SaleVersion>()
                    .AsQueryable()
                    .AnyAsync(p => p.IsInUse == true
                                   && p.PoemId == poemEntity.Id
                                   && p.Status == SaleVersionStatus.Free) ||

                // Allow to upload record file if user is copy right holder
                await _unitOfWork.GetRepository<UsageRight>()
                    .AsQueryable()
                    .AnyAsync(p => p.SaleVersion != null
                                   && p.SaleVersion.PoemId == poemEntity.Id
                                   && p.UserId == userId
                                   && p.Status == UsageRightStatus.StillValid);

            poems.Last().IsAbleToUploadRecordFile = isAbleToUploadRecordFile;
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
        if (anyFreeSaleVersion)
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
        if (poem.UserId == userId)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User is owner of this poem");
        }

        // Get the latest sale version of poem
        SaleVersion? saleVersion = await _unitOfWork.GetRepository<SaleVersion>()
            .FindAsync(p => p.PoemId == poemId && p.Status == SaleVersionStatus.InSale);

        if (saleVersion == null)
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
            Status = UsageRightStatus.StillValid,
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

        // Get purchase user information
        User? purchaseUser = await _unitOfWork.GetRepository<User>()
            .FindAsync(p => p.Id == userId);
        if (purchaseUser != null)
        {
            // Send announcement to UsageRight Holder
            await _publishEndpoint.Publish(new SendUserAnnouncementEvent()
            {
                UserId = saleVersion.Poem.UserId,
                Title = $"Quyền sử dụng bài thơ '{saleVersion.Poem.Title}'",
                Content = $"{purchaseUser.UserName}: đã mua quyền sử dụng bài thơ '{saleVersion.Poem.Title}' của bạn",
                IsRead = false
            });
        }
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

    public async Task<string> ConvertPoemTextToImage(Guid userId, ConvertPoemTextToImageRequest request)
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

        return response.Results.First().Url;
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
        Guid userId, ConvertPoemTextToImageWithTheHiveAiFluxSchnellEnhancedRequest request)
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

    public async Task<string> DownloadAiImageAndUploadToS3Storage(UploadAiPoemImageRequest request, Guid userId)
    {
        var folderName = $"poems/{StringHelper.CapitalizeString(userId.ToString())}";

        return await _awsS3Service.DownloadAndUploadToS3Async(request.ImageUrl, folderName);
    }

    public async Task<TheHiveAiResponse> ConvertPoemTextToImageWithTheHiveAiSdxlEnhanced(
        Guid userId, ConvertPoemTextToImageWithTheHiveAiSdxlEnhancedRequest request)
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

        await _qDrantService.StorePoemEmbeddingAsync(poemId, poem.UserId.Value, poem.Content!);
    }

    public bool IsPoemPlagiarism(double score)
    {
        return score > 0.75;
    }

    public IList<SearchPointsResult> GetListQDrantSearchPoint(QDrantResponse<SearchPointsResult> response, int top)
    {
        var poemPlagiarism = response.Results
            .OrderByDescending(p => p.Score)
            .Take(top)
            .Select(p => new
            {
                Id = p.Id,
                Score = p.Score
            }).ToList();

        // Map to response
        IList<SearchPointsResult> plagiarismFromResponses = poemPlagiarism.Select(p => new SearchPointsResult()
        {
            Id = p.Id,
            Score = p.Score
        }).ToList();

        return plagiarismFromResponses;
    }

    public async Task<PoemPlagiarismResponse> CheckPoemPlagiarism(Guid userId, CheckPoemPlagiarismRequest request)
    {
        // Search similar poem embedding point
        var response = await _qDrantService.SearchSimilarPoemEmbeddingPoint(userId, request.PoemContent);

        // If the score is greater than 0.9 then return the score of highest
        double averageScore = response.Results
            .Where(p => p.Score > 0.9)
            .Select(p => p.Score)
            .DefaultIfEmpty(0.0)
            .Max();

        // If the score is not greater than 0.9 then return the average score
        if (averageScore == 0.0)
        {
            averageScore = response.Results.Select(p => p.Score).Average();
        }

        // Get source plagiarism
        IList<PoemPlagiarismFromResponse> plagiarismFromResponses = new List<PoemPlagiarismFromResponse>();

        /*// Assign top 3 source plagiarism to annonymous object
        var poemPlagiarism = response.Results
            .OrderByDescending(p => p.Score)
            .Take(3)
            .Select(p => new
            {
                Id = p.Id,
                Score = p.Score
            }).ToList();*/
        var qDrantSearchPoint = GetListQDrantSearchPoint(response, 3);

        foreach (var poem in qDrantSearchPoint)
        {
            var poemPlagiarismEntity =
                await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == Guid.Parse(poem.Id));

            // If poem not found then continue to next
            if (poemPlagiarismEntity == null)
            {
                continue;
            }

            // Map to response
            plagiarismFromResponses.Add(_mapper.Map<PoemPlagiarismFromResponse>(poemPlagiarismEntity));

            // Assign author to poem by adding into the last element of the list
            plagiarismFromResponses.Last().User =
                _mapper.Map<GetBasicUserInformationResponse>(poemPlagiarismEntity.User);

            // Map source plagiarism
            plagiarismFromResponses.Last().Score = poem.Score;
        }

        return new PoemPlagiarismResponse()
        {
            Score = averageScore,
            IsPlagiarism = IsPoemPlagiarism(averageScore),
            PlagiarismFrom = IsPoemPlagiarism(averageScore) ? plagiarismFromResponses : null,
        };
    }

    public async Task<PaginationResponse<GetUserPoemResponse>>
        GetUserPoems(Guid? userId, string userName,
            RequestOptionsBase<GetPoemsFilterOption, GetPoemsSortOption> request)
    {
        var poemQuery = _unitOfWork.GetRepository<Poem>().AsQueryable();

        poemQuery = poemQuery.Where(p => p.User!.UserName == userName && p.Status == PoemStatus.Posted);

        // Is delete
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
                poemQuery = poemQuery.OrderByDescending(p => p.CreatedTime).ThenByDescending(p => p.Type);
                break;
        }

        var queryPaging = await _unitOfWork.GetRepository<Poem>()
            .GetPagination(poemQuery, request.PageNumber, request.PageSize);

        //var poems = _mapper.Map<IList<GetPoemResponse>>(queryPaging.Data);

        IList<GetUserPoemResponse> poems = new List<GetUserPoemResponse>();
        foreach (var poem in queryPaging.Data)
        {
            var poemEntity = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poem.Id);
            if (poemEntity == null)
            {
                continue;
            }

            poems.Add(_mapper.Map<GetUserPoemResponse>(poemEntity));
            // Assign author to poem by adding into the last element of the list
            poems.Last().User = _mapper.Map<GetBasicUserInformationResponse>(poemEntity.User);

            // Check if user is able to upload record file for this poem
            bool isAbleToUploadRecordFile =
                // Allow to upload record file if poem is free
                await _unitOfWork.GetRepository<SaleVersion>()
                    .AsQueryable()
                    .AnyAsync(p => p.IsInUse == true
                                   && p.PoemId == poemEntity.Id
                                   && p.Status == SaleVersionStatus.Free) ||

                // Allow to upload record file if user is copy right holder
                await _unitOfWork.GetRepository<UsageRight>()
                    .AsQueryable()
                    .AnyAsync(p => p.SaleVersion != null
                                   && p.SaleVersion.PoemId == poemEntity.Id
                                   && p.UserId == poemEntity.UserId
                                   && p.Status == UsageRightStatus.StillValid);

            poems.Last().IsAbleToUploadRecordFile = isAbleToUploadRecordFile;

            // Search User
            User? user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);
            if (user == null)
            {
                continue;
            }

            // Assign like to poem by adding into the last element of the list
            poems.Last().Like =
                _mapper.Map<GetLikeResponse>(
                    poemEntity.Likes!.FirstOrDefault(l => l.UserId == user.Id && l.PoemId == poemEntity.Id));

            // Assign target mark to poem by adding into the last element of the list
            poems.Last().TargetMark = _mapper.Map<GetTargetMarkResponse>
            (poemEntity.TargetMarks!.FirstOrDefault(tm =>
                tm.MarkByUserId == user.Id && tm.PoemId == poemEntity.Id && tm.Type == TargetMarkType.Poem));
        }

        return new PaginationResponse<GetUserPoemResponse>(poems, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }

    public async Task AdminUpdatePoemStatus(Guid poemId, PoemStatus status)
    {
        Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poemId);

        // If poem not found then throw exception
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }

        poem.Status = status;

        _unitOfWork.GetRepository<Poem>().Update(poem);
        await _unitOfWork.SaveChangesAsync();
    }
}
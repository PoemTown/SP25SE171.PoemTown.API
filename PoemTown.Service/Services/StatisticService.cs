using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Accounts;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.ResponseModels.StatisticResponse;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.StatisticFilters;

namespace PoemTown.Service.Services;

public class StatisticService : IStatisticService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<StatisticResponse> GetStatisticsAsync(Guid userId)
    {
        var collections = _unitOfWork.GetRepository<Collection>()
            .AsQueryable()
            .Where(c => c.UserId == userId && c.DeletedTime == null);

        // Nếu không có bộ sưu tập, trả về thống kê mặc định
        if (!await collections.AnyAsync())
        {
            return new StatisticResponse();
        }

        // Lấy danh sách bài thơ chưa bị xóa từ các bộ sưu tập
        var poemsQuery = collections
            .SelectMany(c => c.Poems)
            .Where(p => p.DeletedTime == null);

        // Truy vấn tuần tự để tránh lỗi DbContext bị sử dụng đồng thời
        var totalCollections = await collections.CountAsync();
        var totalPoems = await poemsQuery.CountAsync();
        var totalRecords = await poemsQuery.SelectMany(p => p.RecordFiles).CountAsync(r => r.DeletedTime == null);
        var totalCollectionBookmarks =
            await collections.SelectMany(p => p.TargetMarks).CountAsync(l => l.DeletedTime == null);
        var totalLikes = await poemsQuery.SelectMany(p => p.Likes).CountAsync(l => l.DeletedTime == null);
        var totalPoemBookmarks = await poemsQuery.SelectMany(p => p.TargetMarks).CountAsync(l => l.DeletedTime == null);

        // Trả về kết quả
        return new StatisticResponse
        {
            TotalCollections = totalCollections,
            TotalPoems = totalPoems,
            TotalLikes = totalLikes,
            TotalPersonalAudios = totalRecords,
            PoemBookmarks = totalPoemBookmarks,
            CollectionBookmarks = totalCollectionBookmarks
        };
    }

    public async Task<GetTotalStatisticResponse> GetTotalStatistic()
    {
        // Lấy tổng số bài thơ đã đăng
        int totalPostedPoems = await _unitOfWork.GetRepository<Poem>()
            .AsQueryable()
            .Where(p => p.DeletedTime == null && p.Status == PoemStatus.Posted)
            .CountAsync();

        // Lấy tổng số file ghi âm
        int totalRecordFiles = await _unitOfWork.GetRepository<RecordFile>()
            .AsQueryable()
            .Where(r => r.DeletedTime == null)
            .CountAsync();

        // Lấy tổng số bộ sưu tập
        int totalCollections = await _unitOfWork.GetRepository<Collection>()
            .AsQueryable()
            .Where(p => p.DeletedTime == null)
            .CountAsync();

        // Lấy tổng số người dùng (Role = User)
        int totalUsers = await _unitOfWork.GetRepository<User>()
            .AsQueryable()
            .Where(p => p.Status == AccountStatus.Active
                        && p.DeletedTime == null
                        && p.UserRoles.Any(r => r.Role.Name == "User"))
            .CountAsync();

        return new GetTotalStatisticResponse
        {
            TotalCollections = totalCollections,
            TotalUsers = totalUsers,
            TotalPostedPoems = totalPostedPoems,
            TotalRecordFiles = totalRecordFiles,
        };
    }

    public async Task<GetOnlineUserStatisticResponse> GetOnlineUserStatistic(GetOnlineUserFilterOption filter)
    {
        var onlineUserQuery = _unitOfWork.GetRepository<LoginTracking>()
            .AsQueryable();

        // Filter by condition: User is active, not yet deleted and login date is less than or equal to current date (UTC + 7)
        onlineUserQuery = onlineUserQuery.Where(p => p.User.Status == AccountStatus.Active
                                                     && p.User.DeletedTime == null
                                                     && p.LoginDate <= DateTimeHelper.SystemTimeNow);
        
        var samples = await (filter.Period switch
        {
            // Filter by date (last 30 days)
            PeriodEnum.ByDate => onlineUserQuery
                .Where(ou => ou.LoginDate.Date >= DateTimeHelper.SystemTimeNow.Date.AddDays(-30)) // Filter last 30 days
                .GroupBy(ou => ou.LoginDate.Date)
                .Select(res => new GetSampleOnlineUserStatisticResponse
                {
                    Year = res.Key.Year,
                    Month = res.Key.Month,
                    Day = res.Key.Day,
                    TotalOnlineUsers = res.Select(p => p.UserId).Distinct().Count()
                })
                .ToListAsync(),

            // Filter by month (within this year)
            PeriodEnum.ByMonth => onlineUserQuery
                .GroupBy(ou => new { ou.LoginDate.Year, ou.LoginDate.Month })
                .Select(res => new GetSampleOnlineUserStatisticResponse
                {
                    Year = res.Key.Year,
                    Month = res.Key.Month,
                    TotalOnlineUsers = res.Select(p => p.UserId).Distinct().Count()
                })
                .ToListAsync(),

            // Filter by year (last 5 years)
            PeriodEnum.ByYear => onlineUserQuery
                .Where(ou => ou.LoginDate.Year >= DateTimeHelper.SystemTimeNow.Year - 5)
                .GroupBy(ou => ou.LoginDate.Year)
                .Select(res => new GetSampleOnlineUserStatisticResponse
                {
                    Year = res.Key,
                    TotalOnlineUsers = res.Select(p => p.UserId).Distinct().Count()
                })
                .ToListAsync(),

            _ => throw new ArgumentOutOfRangeException()
        });

        return new GetOnlineUserStatisticResponse()
        {
            TotalDataSamples = samples.Select(p => p.TotalOnlineUsers).Sum(),
            Samples = samples
        };
    }
}
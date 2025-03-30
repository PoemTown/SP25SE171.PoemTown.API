using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Accounts;
using PoemTown.Repository.Enums.Orders;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;
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

    public async Task<IList<GetSampleStatisticResponse<TValue>>> GetSampleStatisticResponse<T, TValue>(
        IQueryable<T> queryable,
        Func<T, DateTimeOffset> dateSelector,
        Func<T, TValue> valueSelector,
        Func<IEnumerable<TValue>, TValue> aggregateFunction,
        GetStatisticFilterOption filter)
    {
        List<T> rawData = await queryable.ToListAsync(); // Fetch data from DB first

        List<GetSampleStatisticResponse<TValue>> samples;

        switch (filter.Period)
        {
            // Get the last 30 days
            case PeriodEnum.ByDate:
            {
                var startDate = DateTimeHelper.SystemTimeNow.Date.AddDays(-30);
                var dateRange = Enumerable.Range(0, 31)
                    .Select(offset => startDate.AddDays(offset))
                    .ToList();

                var actualData = rawData
                    .Where(p => dateSelector(p).Date >= startDate)
                    .GroupBy(p => dateSelector(p).Date)
                    .Select(res => new GetSampleStatisticResponse<TValue>
                    {
                        Year = res.Key.Year,
                        Month = res.Key.Month,
                        Day = res.Key.Day,
                        TotalSamples = aggregateFunction(res.Select(valueSelector))
                    })
                    .ToList();

                samples = dateRange
                    .Select(date =>
                        actualData.FirstOrDefault(
                            d => d.Year == date.Year && d.Month == date.Month && d.Day == date.Day)
                        ?? new GetSampleStatisticResponse<TValue>
                            { Year = date.Year, Month = date.Month, Day = date.Day, TotalSamples = default })
                    .ToList();
                break;
            }

            // Get the last 12 months
            case PeriodEnum.ByMonth:
            {
                var currentYear = DateTimeHelper.SystemTimeNow.Year;
                var monthRange = Enumerable.Range(1, 12)
                    .Select(month => new { Year = currentYear, Month = month })
                    .ToList();

                var actualData = rawData
                    .Where(p => dateSelector(p).Year == currentYear)
                    .GroupBy(p => new { dateSelector(p).Year, dateSelector(p).Month })
                    .Select(res => new GetSampleStatisticResponse<TValue>
                    {
                        Year = res.Key.Year,
                        Month = res.Key.Month,
                        TotalSamples = aggregateFunction(res.Select(valueSelector))
                    })
                    .ToList();

                samples = monthRange
                    .Select(monthData =>
                        actualData.FirstOrDefault(d => d.Year == monthData.Year && d.Month == monthData.Month)
                        ?? new GetSampleStatisticResponse<TValue>
                            { Year = monthData.Year, Month = monthData.Month, TotalSamples = default })
                    .ToList();
                break;
            }

            // Get the last 5 years
            case PeriodEnum.ByYear:
            {
                var startYear = DateTimeHelper.SystemTimeNow.Year - 4;
                var yearRange = Enumerable.Range(startYear, 5).ToList();

                var actualData = rawData
                    .Where(p => dateSelector(p).Year >= startYear)
                    .GroupBy(p => dateSelector(p).Year)
                    .Select(res => new GetSampleStatisticResponse<TValue>
                    {
                        Year = res.Key,
                        TotalSamples = aggregateFunction(res.Select(valueSelector))
                    })
                    .ToList();

                samples = yearRange
                    .Select(year =>
                        actualData.FirstOrDefault(d => d.Year == year)
                        ?? new GetSampleStatisticResponse<TValue> { Year = year, TotalSamples = default })
                    .ToList();
                break;
            }

            default:
                throw new ArgumentOutOfRangeException();
        }

        return samples;
    }

    public async Task<GetOnlineUserStatisticResponse> GetOnlineUserStatistic(GetOnlineUserFilterOption filter)
    {
        var onlineUserQuery = _unitOfWork.GetRepository<LoginTracking>()
            .AsQueryable();

        // Filter by condition: User is active, not yet deleted and login date is less than or equal to current date (UTC + 7)
        onlineUserQuery = onlineUserQuery.Where(p => p.User.Status == AccountStatus.Active
                                                     && p.User.DeletedTime == null
                                                     && p.LoginDate <= DateTimeHelper.SystemTimeNow);

        var samples = await GetSampleStatisticResponse(
            onlineUserQuery,
            p => p.LoginDate,
            p => 1,
            p => p.Count(),
            new GetStatisticFilterOption()
            {
                Period = filter.Period
            });

        return new GetOnlineUserStatisticResponse
        {
            TotalDataSamples = samples.Select(p => p.TotalSamples).Sum(),
            Samples = samples
        };
    }

    public async Task<GetPoemUploadStatisticResponse> GetUploadPoemStatistic(GetPoemUploadFilterOption filter)
    {
        var poemQuery = _unitOfWork.GetRepository<Poem>()
            .AsQueryable();

        // Filter by condition: Poem is active, not yet deleted and upload date is less than or equal to current date (UTC + 7)
        poemQuery = poemQuery.Where(p => p.Status == PoemStatus.Posted
                                         && p.DeletedTime == null
                                         && p.CreatedTime <= DateTimeHelper.SystemTimeNow);

        var samples = await GetSampleStatisticResponse(poemQuery,
            p => p.CreatedTime,
            p => 1,
            p => p.Count(),
            new GetStatisticFilterOption()
            {
                Period = filter.Period
            });

        return new GetPoemUploadStatisticResponse()
        {
            TotalDataSamples = samples.Select(p => p.TotalSamples).Sum(),
            Samples = samples
        };
    }

    public async Task<GetPoemTypeStatisticResponse> GetPoemTypeStatistic()
    {
        var poemQuery = _unitOfWork.GetRepository<Poem>()
            .AsQueryable();

        // Filter by condition: Poem is active, not yet deleted and upload date is less than or equal to current date (UTC + 7)
        poemQuery = poemQuery.Where(p => p.Status == PoemStatus.Posted
                                         && p.DeletedTime == null
                                         && p.CreatedTime <= DateTimeHelper.SystemTimeNow);

        var samples = await poemQuery
            .GroupBy(p => p.Type)
            .Select(res => new GetPoemTypeSampleResponse
            {
                Type = res.Key,
                TotalPoems = res.Count()
            })
            .ToListAsync();

        return new GetPoemTypeStatisticResponse()
        {
            TotalDataSamples = samples.Select(p => p.TotalPoems).Sum(),
            Samples = samples
        };
    }

    public async Task<GetReportPoemStatisticResponse> GetReportPoemStatistic()
    {
        // query by poem report
        var reportPoemQuery = _unitOfWork.GetRepository<Report>()
            .AsQueryable()
            .Where(p => p.Poem != null && p.ReportedUser == null);

        // group by status
        var samples = await reportPoemQuery
            .GroupBy(p => p.Status)
            .Select(p => new GetReportPoemSampleResponse()
            {
                Type = p.Key,
                TotalPoems = p.Count()
            })
            .ToListAsync();

        return new GetReportPoemStatisticResponse()
        {
            TotalDataSamples = samples.Select(p => p.TotalPoems).Sum(),
            Samples = samples,
        };
    }

    public async Task<GetReportUserStatisticResponse> GetReportUserStatistic()
    {
        // query by user report
        var reportUserQuery = _unitOfWork.GetRepository<Report>()
            .AsQueryable()
            .Where(p => p.Poem == null && p.ReportedUser != null);

        // group by status
        var samples = await reportUserQuery
            .GroupBy(p => p.Status)
            .Select(p => new GetReportUserSampleResponse()
            {
                Type = p.Key,
                TotalUsers = p.Count()
            })
            .ToListAsync();

        return new GetReportUserStatisticResponse()
        {
            TotalDataSamples = samples.Select(p => p.TotalUsers).Sum(),
            Samples = samples,
        };
    }

    public async Task<GetReportPoemStatisticResponse> GetReportPlagiarismPoemStatistic()
    {
        // query by plagiarism poem report
        var reportPoemQuery = _unitOfWork.GetRepository<Report>()
            .AsQueryable()
            .Where(p => p.Poem != null
                        && p.PlagiarismPoemReports != null
                        && p.ReportedUser == null);

        // group by status
        var samples = await reportPoemQuery
            .GroupBy(p => p.Status)
            .Select(p => new GetReportPoemSampleResponse()
            {
                Type = p.Key,
                TotalPoems = p.Count()
            })
            .ToListAsync();

        return new GetReportPoemStatisticResponse()
        {
            TotalDataSamples = samples.Select(p => p.TotalPoems).Sum(),
            Samples = samples,
        };
    }

    public async Task<GetTransactionStatisticResponse> GetTransactionStatistic(
        GetTransactionStatisticFilterOption filter)
    {
        var transactionQuery = _unitOfWork.GetRepository<Transaction>()
            .AsQueryable();

        // Filter by condition: CreatedTime is less than or equal to current date (UTC + 7)
        transactionQuery = transactionQuery.Where(p => p.CreatedTime <= DateTimeHelper.SystemTimeNow);

        // Transaction samples (total transactions)
        var samples = await GetSampleStatisticResponse(
            transactionQuery, p => p.CreatedTime,
            p => 1,
            p => p.Count(),
            new GetStatisticFilterOption()
            {
                Period = filter.Period
            });

        // Transaction amounts (total amounts)
        var transactionAmount = await GetSampleStatisticResponse(
            transactionQuery, p => p.CreatedTime,
            p => p.Amount,
            p => p.Sum(),
            new GetStatisticFilterOption()
            {
                Period = filter.Period
            });

        return new GetTransactionStatisticResponse
        {
            Samples = new GetStatisticResponse<int>
            {
                TotalDataSamples = samples.Sum(p => p.TotalSamples),
                Samples = samples
            },
            Amounts = new GetStatisticResponse<decimal>
            {
                TotalDataSamples = transactionAmount.Sum(p => p.TotalSamples),
                Samples = transactionAmount
            }
        };
    }

    public async Task<GetOrderStatusStatisticResponse> GetOrderStatusStatistic()
    {
        var orderQuery = _unitOfWork.GetRepository<Order>()
            .AsQueryable();

        // Filter by condition: CreatedTime is less than or equal to current date (UTC + 7)
        orderQuery = orderQuery.Where(p => p.CreatedTime <= DateTimeHelper.SystemTimeNow);

        var actualData = await orderQuery
            .GroupBy(p => p.Status)
            .Select(res => new GetOrderTypeSampleResponse()
            {
                Status = res.Key,
                TotalOrders = res.Count()
            })
            .ToListAsync();

        var allOrderStatus = new[]
        {
            OrderStatus.Paid,
            OrderStatus.Pending,
            OrderStatus.Cancelled
        };
        
        // Group by order type
        var samples = allOrderStatus
            .Select(status => actualData.FirstOrDefault(p => p.Status == status)
                             ?? new GetOrderTypeSampleResponse()
                             {
                                 Status = status,
                                 TotalOrders = 0
                             })
            .ToList();

        return new GetOrderStatusStatisticResponse()
        {
            TotalDataSamples = samples.Select(p => p.TotalOrders).Sum(),
            Samples = samples
        };
    }

    public async Task<GetMasterTemplateOrderStatisticResponse> GetMasterTemplateOrderStatistic()
    {
        var masterTemplateOrderQuery = _unitOfWork.GetRepository<OrderDetail>()
            .AsQueryable();

        // Filter by condition: mastertemplate is not null, with status paid and created date is less than or equal to current date (UTC + 7)
        masterTemplateOrderQuery = masterTemplateOrderQuery.Where(p => p.MasterTemplate != null
                                                                       && p.CreatedTime <= DateTimeHelper.SystemTimeNow
                                                                       && p.Order.Status == OrderStatus.Paid);

        // Get all mastertemplate names
        var allTemplateNames = await _unitOfWork.GetRepository<MasterTemplate>()
            .AsQueryable()
            .Where(p => p.DeletedTime == null)
            .Select(p => new{ p.TemplateName, p.TagName })
            .ToListAsync();
        
        // Get actual data from database
        var actualData = await masterTemplateOrderQuery
            .GroupBy(p => p.MasterTemplate!.TemplateName)
            .Select(res => new GetMasterTemplateOrderSampleResponse()
            {
                TemplateName = res.Key ?? "",
                TotalOrders = res.Count()
            })
            .ToListAsync();
        
        
        // Group by mastertemplate name
        var samples = allTemplateNames
            .Select(template => actualData.FirstOrDefault(p => p.TemplateName == template.TemplateName)
                                   ?? new GetMasterTemplateOrderSampleResponse()
                                   {
                                       TemplateName = template.TemplateName ?? "",
                                       TagName = template.TagName ?? "",
                                       TotalOrders = 0
                                   })
            .ToList();
        
        return new GetMasterTemplateOrderStatisticResponse()
        {
            TotalDataSamples = samples.Select(p => p.TotalOrders).Sum(),
            Samples = samples
        };
    }
}
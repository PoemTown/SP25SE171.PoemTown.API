using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Accounts;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Repository.Enums.Reports;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.ReportRequests;
using PoemTown.Service.BusinessModels.ResponseModels.ReportResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.PlagiarismDetector.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.ReportFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.ReportSorts;

namespace PoemTown.Service.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IQDrantService _qDrantService;

    public ReportService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IQDrantService qDrantService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _qDrantService = qDrantService;
    }

    public async Task CreateReportPoem(Guid userId, CreateReportPoemRequest request)
    {
        // Check if the poem exists
        var poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == request.PoemId);
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }

        if (poem.Status != PoemStatus.Posted)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Can not report poem which is not posted");
        }

        // Check if the user reported his own poem, if so, throw an exception
        if (poem.UserId == userId)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "You can't report your own poem");
        }

        var report = new Report
        {
            PoemId = request.PoemId,
            ReportReason = request.ReportReason,
            Type = ReportType.Poem,
            ReportUserId = userId,
        };

        await _unitOfWork.GetRepository<Report>().InsertAsync(report);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PaginationResponse<GetReportResponse>>
        GetReports(RequestOptionsBase<GetReportFilterOption, GetReportSortOption> request)
    {
        var reportQuery = _unitOfWork.GetRepository<Report>().AsQueryable();

        // Filter the reports
        if (request.FilterOptions != null)
        {
            if (request.FilterOptions.Status != null)
            {
                reportQuery = reportQuery.Where(r => r.Status == request.FilterOptions.Status);
            }

            if (!string.IsNullOrEmpty(request.FilterOptions.ReportUserEmail))
            {
                reportQuery = reportQuery.Where(r => r.ReportUser!.Email!.Trim().ToLower()
                    .Contains(request.FilterOptions.ReportUserEmail.Trim().ToLower()));
            }
        }

        // Sort the reports
        reportQuery = request.SortOptions switch
        {
            GetReportSortOption.CreatedTimeAscending => reportQuery.OrderBy(r => r.CreatedTime),
            GetReportSortOption.CreatedTimeDescending => reportQuery.OrderByDescending(r => r.CreatedTime),
            _ => reportQuery.OrderBy(r => r.CreatedTime).ThenBy(p => p.ReportedUser)
        };

        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<Report>()
            .GetPagination(reportQuery, request.PageNumber, request.PageSize);

        IList<GetReportResponse> reports = new List<GetReportResponse>();

        foreach (var report in queryPaging.Data)
        {
            Report? reportEntity = await _unitOfWork.GetRepository<Report>().FindAsync(p => p.Id == report.Id);
            if (reportEntity == null)
            {
                continue;
            }

            reports.Add(_mapper.Map<GetReportResponse>(reportEntity));

            // Get information of the user who reported the report
            reports.Last().ReportReportUser = _mapper.Map<GetReportUserInReportResponse>(reportEntity.ReportUser);
            reports.Last().Poem = _mapper.Map<GetPoemInReportResponse>(reportEntity.Poem);

            IList<PlagiarismPoemReport>? plagiarismPoemReports = await _unitOfWork.GetRepository<PlagiarismPoemReport>()
                .AsQueryable()
                .Where(p => p.ReportId == reportEntity.Id)
                .ToListAsync();

            reports.Last().PlagiarismFromPoems = _mapper.Map<IList<GetPoemInReportResponse>>
                (plagiarismPoemReports.Select(p => p.PlagiarismFromPoem));

            // Get the plagiarism score in the plagiarism poems
            var getPoemInReportResponses = reports.Last().PlagiarismFromPoems;

            getPoemInReportResponses?.ToList().ForEach(p =>
            {
                var plagiarismPoemReport =
                    plagiarismPoemReports.FirstOrDefault(pr => pr.PlagiarismFromPoemId == p.Id);
                if (plagiarismPoemReport != null)
                {
                    p.Score = plagiarismPoemReport.Score;
                }
            });
        }

        return new PaginationResponse<GetReportResponse>
        (reports, queryPaging.PageNumber, queryPaging.PageSize, queryPaging.TotalRecords,
            queryPaging.CurrentPageRecords);
    }


    public async Task<PaginationResponse<GetMyReportResponse>>
        GetMyReports(Guid userId, RequestOptionsBase<GetMyReportFilterOption, GetMyReportSortOption> request)
    {
        var reportQuery = _unitOfWork.GetRepository<Report>().AsQueryable();

        reportQuery = reportQuery.Where(p => p.ReportUserId == userId);
        // Filter the reports
        if (request.FilterOptions != null)
        {
            if (request.FilterOptions.Status != null)
            {
                reportQuery = reportQuery.Where(r => r.Status == request.FilterOptions.Status);
            }
        }

        // Sort the reports
        reportQuery = request.SortOptions switch
        {
            GetMyReportSortOption.CreatedTimeAscending => reportQuery.OrderBy(r => r.CreatedTime),
            GetMyReportSortOption.CreatedTimeDescending => reportQuery.OrderByDescending(r => r.CreatedTime),
            _ => reportQuery.OrderBy(r => r.CreatedTime)
        };

        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<Report>()
            .GetPagination(reportQuery, request.PageNumber, request.PageSize);

        IList<GetMyReportResponse> reports = new List<GetMyReportResponse>();

        foreach (var report in queryPaging.Data)
        {
            Report? reportEntity = await _unitOfWork.GetRepository<Report>().FindAsync(p => p.Id == report.Id);
            if (reportEntity == null)
            {
                continue;
            }

            reports.Add(_mapper.Map<GetMyReportResponse>(reportEntity));

            // Get information of the user who reported the report
            reports.Last().Poem = _mapper.Map<GetPoemInReportResponse>(reportEntity.Poem);
        }

        return new PaginationResponse<GetMyReportResponse>
        (reports, queryPaging.PageNumber, queryPaging.PageSize, queryPaging.TotalRecords,
            queryPaging.CurrentPageRecords);
    }

    public async Task ResolveReport(ResolveReportRequest request)
    {
        var report = await _unitOfWork.GetRepository<Report>().FindAsync(r => r.Id == request.Id);

        // Check if the report exists
        if (report == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Report not found");
        }

        // Resolve the report of the poem
        if (report.Type == ReportType.Poem)
        {
            Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == report.PoemId);

            // Check if the poem exists
            if (poem == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
            }
        }

        // Resolve the report of the user
        else if (report.Type == ReportType.User)
        {
            User? user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == report.ReportedUserId);

            // Check if the user exists
            if (user == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
            }
        }

        // Resolve the report of the plagiarism
        else if (report.Type == ReportType.Plagiarism)
        {
            Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == report.PoemId);

            // Check if the poem exists
            if (poem == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
            }

            // Check if the poem is not suspended
            if (poem.Status != PoemStatus.Pending)
            {
                throw new CoreException(StatusCodes.Status400BadRequest,
                    "Can not approve report of poem which is not pending");
            }

            switch (request.Status)
            {
                case ReportStatus.Approved:
                    poem.Status = PoemStatus.Posted;

                    // Store the poem embedding
                    await _qDrantService.StorePoemEmbeddingAsync(poem.Id, poem.UserId!.Value, poem.Content!);
                    break;
                case ReportStatus.Rejected:
                    poem.Status = PoemStatus.Draft;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Update the report
        report.ResolveResponse = request.ResolveResponse;
        report.Status = request.Status;

        _unitOfWork.GetRepository<Report>().Update(report);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CreateReportUser(Guid userId, CreateReportUserRequest request)
    {
        // Check if the user exists
        var user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == request.UserId);
        if (user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "user not found");
        }

        // Check if the reported user is not active
        if (user.Status != AccountStatus.Active)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Can not report user which is not active");
        }

        // Check if the user reported his own profile, if so, throw an exception
        if (user.Id == userId)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "You can't report your own profile");
        }

        var report = new Report
        {
            ReportedUserId = request.UserId,
            ReportReason = request.ReportReason,
            Type = ReportType.User,
            ReportUserId = userId,
        };

        await _unitOfWork.GetRepository<Report>().InsertAsync(report);
        await _unitOfWork.SaveChangesAsync();
    }
}
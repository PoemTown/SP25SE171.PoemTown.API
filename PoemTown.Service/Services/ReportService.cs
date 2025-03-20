using AutoMapper;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Repository.Enums.Reports;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.ReportRequests;
using PoemTown.Service.BusinessModels.ResponseModels.ReportResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.ReportFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.ReportSorts;

namespace PoemTown.Service.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
        if(poem.Collection!.UserId == userId)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "You can't report your own poem");
        }
        
        var report = new Report
        {
            PoemId = request.PoemId,
            ReportReason = request.ReportReason,
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
            reports.Last().PlagiarismFromPoem = _mapper.Map<GetPoemInReportResponse>(reportEntity.PlagiarismFromPoem);
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
}
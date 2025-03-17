using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.ReportRequests;
using PoemTown.Service.BusinessModels.ResponseModels.ReportResponses;
using PoemTown.Service.QueryOptions.FilterOptions.ReportFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.ReportSorts;

namespace PoemTown.Service.Interfaces;

public interface IReportService
{
    Task CreateReportPoem(Guid userId, CreateReportPoemRequest request);

    Task<PaginationResponse<GetReportResponse>>
        GetReports(RequestOptionsBase<GetReportFilterOption, GetReportSortOption> request);

    Task<PaginationResponse<GetMyReportResponse>>
        GetMyReports(Guid userId, RequestOptionsBase<GetMyReportFilterOption, GetMyReportSortOption> request);
}
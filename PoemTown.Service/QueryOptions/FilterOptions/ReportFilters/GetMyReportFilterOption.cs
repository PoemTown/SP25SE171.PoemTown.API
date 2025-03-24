using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.Reports;

namespace PoemTown.Service.QueryOptions.FilterOptions.ReportFilters;

public class GetMyReportFilterOption
{
    [FromQuery(Name = "reportStatus")]
    public ReportStatus? Status { get; set; }
}
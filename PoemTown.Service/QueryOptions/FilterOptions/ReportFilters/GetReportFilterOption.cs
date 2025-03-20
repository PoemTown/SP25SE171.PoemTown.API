using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.Reports;

namespace PoemTown.Service.QueryOptions.FilterOptions.ReportFilters;

public class GetReportFilterOption
{
    [FromQuery(Name = "reportStatus")]
    public ReportStatus? Status { get; set; }
    [FromQuery(Name = "reportUserEmail")]
    public string? ReportUserEmail { get; set; }
    
    [FromQuery(Name = "isSystem")]
    public bool? IsSystem { get; set; }
}
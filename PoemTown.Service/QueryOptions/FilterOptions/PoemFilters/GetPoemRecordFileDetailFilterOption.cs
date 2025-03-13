using Microsoft.AspNetCore.Mvc;

namespace PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;

public class GetPoemRecordFileDetailFilterOption
{
    [FromQuery(Name = "fileName")]
    public string? FileName { get; set; }
}
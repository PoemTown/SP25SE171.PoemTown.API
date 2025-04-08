using Microsoft.AspNetCore.Mvc;

namespace PoemTown.Service.QueryOptions.FilterOptions.PoetSampleFilters;

public class GetPoetSampleFilterOption
{
    [FromQuery(Name = "name")]
    public string? Name { get; set; }
}
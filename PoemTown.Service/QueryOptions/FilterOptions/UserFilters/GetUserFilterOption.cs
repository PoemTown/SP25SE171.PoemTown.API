using Microsoft.AspNetCore.Mvc;

namespace PoemTown.Service.QueryOptions.FilterOptions.UserFilters;

public class GetUserFilterOption
{
    [FromQuery(Name = "userName")]
    public string? UserName { get; set; }
}
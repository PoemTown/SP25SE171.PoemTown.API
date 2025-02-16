using Microsoft.AspNetCore.Mvc;

namespace PoemTown.Service.QueryOptions.FilterOptions.FollowerFilters;

public class GetFollowersFilterOption
{
    [FromQuery(Name = "displayName")]
    public string? DisplayName { get; set; }
}
using Microsoft.AspNetCore.Mvc;

namespace PoemTown.Service.QueryOptions.FilterOptions.ThemeFilters;

public class GetUserThemeFilterOption
{
    [FromQuery(Name = "isInUse")]
    public bool? IsInUse { get; set; }
    [FromQuery(Name = "isDefault")]
    public bool? IsDefault { get; set; }
}
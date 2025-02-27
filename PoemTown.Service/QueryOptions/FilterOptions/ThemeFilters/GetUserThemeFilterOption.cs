using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.TemplateDetails;

namespace PoemTown.Service.QueryOptions.FilterOptions.ThemeFilters;

public class GetUserThemeFilterOption
{
    [FromQuery(Name = "isInUse")]
    public bool? IsInUse { get; set; }
    [FromQuery(Name = "isDefault")]
    public bool? IsDefault { get; set; }
    [FromQuery(Name = "templateDetailType")]
    public TemplateDetailType TemplateDetailType { get; set; }
}
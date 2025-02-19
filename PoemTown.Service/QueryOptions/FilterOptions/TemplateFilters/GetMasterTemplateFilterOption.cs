using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.Templates;
using PoemTown.Service.QueryOptions.ExtensionOptions;

namespace PoemTown.Service.QueryOptions.FilterOptions.TemplateFilters;

public class GetMasterTemplateFilterOption
{
    [FromQuery(Name = "templateName")]
    public string? TemplateName { get; set; }
    [FromQuery(Name = "tagName")]
    public string? TagName { get; set; }
    [FromQuery(Name = "type")]
    public TemplateType? Type { get; set; }
    [FromQuery(Name = "priceRange")]
    public PriceRange? PriceRange { get; set; }
}
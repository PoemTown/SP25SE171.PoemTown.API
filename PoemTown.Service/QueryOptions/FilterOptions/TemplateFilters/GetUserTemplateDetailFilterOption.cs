using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.TemplateDetails;
using PoemTown.Repository.Enums.Templates;

namespace PoemTown.Service.QueryOptions.FilterOptions.TemplateFilters;

public class GetUserTemplateDetailFilterOption
{
    [FromQuery(Name = "type")]
    public TemplateDetailType Type { get; set; }
    [FromQuery(Name = "templateType")]
    public TemplateType TemplateType { get; set; }
}
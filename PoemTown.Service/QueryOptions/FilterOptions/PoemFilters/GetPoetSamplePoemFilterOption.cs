using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.Poems;

namespace PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;

public class GetPoetSamplePoemFilterOption
{
    [FromQuery(Name = "status")]
    public PoemStatus? Status { get; set; } = PoemStatus.Posted;
    [FromQuery(Name = "title")]
    public string? Title { get; set; }

    [FromQuery(Name = "type")]
    public Guid? PoemTypeId { get; set; }
}
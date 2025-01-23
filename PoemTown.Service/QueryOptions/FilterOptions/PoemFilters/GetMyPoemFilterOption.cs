using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums;
using PoemTown.Repository.Enums.Poems;

namespace PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;

public class GetMyPoemFilterOption
{
    [FromQuery(Name = "collectionId")]
    public Guid? CollectionId { get; set; }
    
    [FromQuery(Name = "chapterName")]
    public string? ChapterName { get; set; }
    
    [FromQuery(Name = "title")]
    public string? Title { get; set; }
    
    [FromQuery(Name = "type")]
    public PoemType? Type { get; set; }
    
    [FromQuery(Name = "status")]
    public PoemStatus? Status { get; set; }
}
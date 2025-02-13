using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PoemTown.Service.CustomAttribute;

namespace PoemTown.Service.QueryOptions.RequestOptions;

public class RequestOptionsBase <TFilterOption, TSortOption>
{
    [FromQuery(Name = "filterOptions")]
    public TFilterOption? FilterOptions { get; set; }
    
    [FromQuery(Name = "sortOptions")]
    public TSortOption? SortOptions { get; set; } = default;
        
    [FromQuery(Name = "isDelete")]
    public bool? IsDelete { get; set; } = false;
    
    [FromQuery(Name = "pageNumber")]
    public int PageNumber { get; set; } = 1;

    [FromQuery(Name = "pageSize")]
    /*
    [Range(1, 250, ErrorMessage = "Page size must be between 1 and 250")]
    */
    [DynamicPageSize]
    public int PageSize { get; set; } = 10;
    [FromQuery(Name = "allowExceedPageSize")]
    public bool AllowExceedPageSize { get; set; } = false;
}
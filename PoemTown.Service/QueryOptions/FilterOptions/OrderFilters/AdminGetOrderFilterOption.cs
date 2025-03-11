using Microsoft.AspNetCore.Mvc;

namespace PoemTown.Service.QueryOptions.FilterOptions.OrderFilters;

public class AdminGetOrderFilterOption : GetOrderFilterOption
{
    [FromQuery(Name = "email")]
    public string? Email { get; set; }
    [FromQuery(Name = "phoneNumber")]
    public string? PhoneNumber { get; set; }
    [FromQuery(Name = "orderCode")]
    public string? OrderCode { get; set; }
}
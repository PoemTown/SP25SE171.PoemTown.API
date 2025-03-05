using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.Orders;

namespace PoemTown.Service.QueryOptions.FilterOptions.OrderFilters;

public class GetOrderFilterOption
{
    [FromQuery(Name = "type")]
    public OrderType? Type { get; set; }
    
    [FromQuery(Name = "status")]
    public OrderStatus? Status { get; set; }
}
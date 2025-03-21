using Microsoft.AspNetCore.Mvc;

namespace PoemTown.Service.QueryOptions.FilterOptions.StatisticFilters;

public class GetOnlineUserFilterOption
{
    [FromQuery(Name = "period")] 
    public PeriodEnum Period { get; set; } = PeriodEnum.ByDate;
}
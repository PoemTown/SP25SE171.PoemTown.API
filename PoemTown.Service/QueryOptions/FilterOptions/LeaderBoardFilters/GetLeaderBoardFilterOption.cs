using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.QueryOptions.FilterOptions.LeaderBoardFilters
{
    public class GetLeaderBoardFilterOption
    {
        [FromQuery(Name = "date")]
        public DateTimeOffset? Date { get; set; }
        [FromQuery(Name = "name")]
        public string? Name { get; set; }
    }
}

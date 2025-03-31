using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Repository.Enums.UsageRights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.QueryOptions.FilterOptions.UsageRightFilters
{
    public class GetUsageRightPoemFilter
    {
        [FromQuery(Name = "status")]
        public UsageRightStatus? UsageRightStatus { get; set; }

        [FromQuery(Name = "poemName")]
        public string? PoemName { get; set; }
    }
}

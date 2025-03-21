using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.Achievements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.QueryOptions.FilterOptions.AchievementFilters
{
    public class GetAchievementFilterOption
    {
        [FromQuery(Name = "type")]
        public AchievementType? Type { get; set; }
        [FromQuery(Name = "rank")]
        public int? Rank { get; set; }
    }
}

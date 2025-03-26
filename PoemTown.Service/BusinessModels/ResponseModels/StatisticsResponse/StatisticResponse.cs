using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse
{
    public class StatisticResponse
    {
        public int TotalPoems { get; set; } = 0;
        public int TotalCollections { get; set; } = 0;
        public int TotalPersonalAudios { get; set; } = 0;
        public int TotalLikes { get; set; } = 0;
        public int PoemBookmarks { get; set; } = 0;
        public int CollectionBookmarks { get; set; } = 0;
    }
}

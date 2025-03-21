using PoemTown.Repository.Enums.Achievements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.ResponseModels.AchievementRespponses
{
    public class GetAchievementResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AchievementType Type { get; set; }
        public int Rank { get; set; }
        public DateTimeOffset EarnedDate { get; set; }
        public Guid UserId { get; set; }

    }
}

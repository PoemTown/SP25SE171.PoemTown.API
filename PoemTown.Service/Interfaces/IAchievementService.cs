using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.ResponseModels.AchievementRespponses;
using PoemTown.Service.QueryOptions.FilterOptions.AchievementFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.AchievementSorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Interfaces
{
    public interface IAchievementService
    {
        Task CreateMonthlyAchievementsAsync();
        Task<PaginationResponse<GetAchievementResponse>> GetMyAchievements(Guid userId, RequestOptionsBase<GetAchievementFilterOption, GetAchievementSortOption> request);
    }
}

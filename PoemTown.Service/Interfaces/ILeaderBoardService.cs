using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.ResponseModels.LeaderBoardResponses;
using PoemTown.Service.QueryOptions.FilterOptions.LeaderBoardFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.LeaderBoardSorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Interfaces
{
    public interface ILeaderBoardService
    {
        Task CalculateTopPoemsAsync();
        Task CalculateTopUsersAsync();

        Task<PaginationResponse<GetLeaderBoardResponse>> GetTopPoemsLeaderBoard(RequestOptionsBase<GetLeaderBoardFilterOption, GetLeaderBoardSortOption> request);
        Task<PaginationResponse<GetLeaderBoardResponse>> GetTopUsersLeaderBoard(RequestOptionsBase<GetLeaderBoardFilterOption, GetLeaderBoardSortOption> request);

    }
}

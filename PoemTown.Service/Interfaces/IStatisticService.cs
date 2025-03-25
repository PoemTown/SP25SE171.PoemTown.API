using PoemTown.Service.BusinessModels.ResponseModels.StatisticResponse;
using PoemTown.Service.QueryOptions.FilterOptions.StatisticFilters;

namespace PoemTown.Service.Interfaces;

public interface IStatisticService
{
    Task<StatisticResponse> GetStatisticsAsync(Guid userId);
    Task<GetTotalStatisticResponse> GetTotalStatistic();
    Task<GetOnlineUserStatisticResponse> GetOnlineUserStatistic(GetOnlineUserFilterOption filter);
    Task<GetPoemUploadStatisticResponse> GetUploadPoemStatistic(GetPoemUploadFilterOption filter);
    Task<GetPoemTypeStatisticResponse> GetPoemTypeStatistic();
}
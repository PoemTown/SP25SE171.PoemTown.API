using PoemTown.Service.BusinessModels.ResponseModels.StatisticResponse;

namespace PoemTown.Service.Interfaces;

public interface IStatisticService
{
    Task<StatisticResponse> GetStatisticsAsync(Guid userId);
}
using PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;
using PoemTown.Service.QueryOptions.FilterOptions.StatisticFilters;

namespace PoemTown.Service.Interfaces;

public interface IStatisticService
{
    Task<StatisticResponse> GetStatisticsAsync(Guid userId);
    Task<GetTotalStatisticResponse> GetTotalStatistic();
    Task<GetOnlineUserStatisticResponse> GetOnlineUserStatistic(GetOnlineUserFilterOption filter);
    Task<GetPoemUploadStatisticResponse> GetUploadPoemStatistic(GetPoemUploadFilterOption filter);
    Task<GetPoemTypeStatisticResponse> GetPoemTypeStatistic();
    Task<GetReportPoemStatisticResponse> GetReportPoemStatistic();
    Task<GetReportUserStatisticResponse> GetReportUserStatistic();
    Task<GetReportPoemStatisticResponse> GetReportPlagiarismPoemStatistic();
    Task<GetTransactionStatisticResponse> GetTransactionStatistic(GetTransactionStatisticFilterOption filter);
    Task<GetOrderStatusStatisticResponse> GetOrderStatusStatistic();
    Task<GetMasterTemplateOrderStatisticResponse> GetMasterTemplateOrderStatistic();
    Task<GetOrderTypeStatisticResponse> GetOrderTypeStatistic();
    Task<GetIncomeStatisticResponse> GetIncomeStatistic(GetIncomeStatisticFilterOption filter);
}
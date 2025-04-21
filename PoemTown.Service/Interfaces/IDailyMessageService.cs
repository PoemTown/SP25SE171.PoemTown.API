using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.DailyMessageRequests;
using PoemTown.Service.BusinessModels.ResponseModels.DailyMessageResponses;
using PoemTown.Service.QueryOptions.FilterOptions.DailyMessageFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.DailyMessageSorts;

namespace PoemTown.Service.Interfaces;

public interface IDailyMessageService
{
    Task CreateNewDailyMessage(CreateNewDailyMessageRequest request);
    Task UpdateDailyMessage(UpdateDailyMessageRequest request);
    Task DeleteDailyMessage(Guid dailyMessageId);
    Task<GetDailyMessageResponse> GetDailyMessage(Guid dailyMessageId);
    Task<GetDailyMessageResponse?> GetInUseDailyMessage();

    Task<PaginationResponse<GetDailyMessageResponse>>
        GetDailyMessages(RequestOptionsBase<GetDailyMessageFilterOption, GetDailyMessageSortOption> request);
}
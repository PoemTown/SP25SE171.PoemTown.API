using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.DepositCommissionSettingRequests;
using PoemTown.Service.BusinessModels.ResponseModels.DepositCommissionSettingResponses;
using PoemTown.Service.QueryOptions.FilterOptions.DepositCommissionSettingFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.DepositCommissionSettingSorts;

namespace PoemTown.Service.Interfaces;

public interface IDepositCommissionSettingService
{
    Task CreateNewDepositCommissionSetting(CreateNewDepositCommissionSettingRequest request);
    Task UpdateDepositCommissionSetting(UpdateDepositCommissionSettingRequest request);
    Task DeleteDepositCommissionSetting(Guid id);

    Task<PaginationResponse<GetDepositCommissionSettingsResponse>>
        GetDepositCommissionSettings(
            RequestOptionsBase<GetDepositCommissionSettingFilterOption, GetDepositCommissionSettingSortOption> request);

    Task<GetDepositCommissionSettingsResponse> GetDepositCommissionSettingDetail(Guid id);
    Task<GetDepositCommissionSettingsResponse> GetInUseDepositCommissionSetting();
}
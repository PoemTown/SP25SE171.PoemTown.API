using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;
using PoemTown.Service.QueryOptions.FilterOptions.OrderFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.OrderSorts;

namespace PoemTown.Service.Interfaces;

public interface IOrderService
{
    Task<PaginationResponse<GetOrderResponse>>
        GetOrders(Guid userId, RequestOptionsBase<GetOrderFilterOption, GetOrderSortOption> request);

    Task<GetDetailsOfOrderResponse> GetOrderDetail(Guid orderId);
}
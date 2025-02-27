using PoemTown.Service.BusinessModels.RequestModels.UserEWalletRequests;
using PoemTown.Service.BusinessModels.ResponseModels.UserEWalletResponses;
using PoemTown.Service.ThirdParties.Models.ZaloPay;
using PoemTown.Service.ThirdParties.Settings.ZaloPay;

namespace PoemTown.Service.ThirdParties.Interfaces;

public interface IZaloPayService
{
    Task<ZaloPayResponseData> CreateOrderUrl<TItem>(OrderCreationSettings<TItem> orderCreationSettings);
}
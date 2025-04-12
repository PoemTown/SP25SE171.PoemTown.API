using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.PaymentGatewayRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PaymentGatewayResponses;
using PoemTown.Service.QueryOptions.FilterOptions.PaymentGatewayFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PaymentGatewaySorts;

namespace PoemTown.Service.Interfaces;

public interface IPaymentGatewayService
{
    Task<PaginationResponse<GetPaymentGatewayResponse>>
        GetPaymentGateways(RequestOptionsBase<GetPaymentGatewayFilterOptions, GetPaymentGatewaySortOptions> request);

    Task CreatePaymentGateway(CreatePaymentGatewayRequest request);
    Task<string> UploadPaymentGatewayIcon(IFormFile file);
    Task UpdatePaymentGateway(UpdatePaymentGatewayRequest request);
}
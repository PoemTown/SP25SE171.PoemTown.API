using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.PaymentGatewayRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PaymentGatewayResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.PaymentGatewayFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PaymentGatewaySorts;

namespace PoemTown.API.Controllers;

public class PaymentGatewaysController : BaseController
{
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly IMapper _mapper;
    public PaymentGatewaysController(IPaymentGatewayService paymentGatewayService, IMapper mapper)
    {
        _paymentGatewayService = paymentGatewayService;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("v1")]
    public async Task<ActionResult<BasePaginationResponse<GetPaymentGatewayResponse>>> GetPaymentGateways(
        [FromQuery] RequestOptionsBase<GetPaymentGatewayFilterOptions, GetPaymentGatewaySortOptions> request)
    {
        var paginationResponse = await _paymentGatewayService.GetPaymentGateways(request);
        
        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetPaymentGatewayResponse>>(paginationResponse);
        basePaginationResponse.Message = "Get payment gateways successfully";
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        
        return Ok(basePaginationResponse);
    }
    
    [HttpPost]
    [Route("v1")]
    public async Task<ActionResult<BaseResponse>> CreatePaymentGateway(CreatePaymentGatewayRequest request)
    {
        await _paymentGatewayService.CreatePaymentGateway(request);
        return Ok(new BaseResponse(StatusCodes.Status201Created, "Payment gateway created successfully"));
    }
}
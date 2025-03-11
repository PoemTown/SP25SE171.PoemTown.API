using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.OrderFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.OrderSorts;

namespace PoemTown.API.Controllers;

public class OrdersController : BaseController
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    
    public OrdersController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Lấy danh sách đơn hàng của người dùng, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// filter.orderType:
    ///
    /// - EWalletDeposit = 1,
    /// - MasterTemplates = 2,
    /// - RecordFiles = 3,
    /// - Poems = 4,
    ///
    /// filter.orderStatus:
    ///
    /// - Pending = 1,
    /// - Paid = 2,
    /// - Cancelled = 3
    ///
    /// sortOptions:
    ///
    /// - OrderDateAscending = 1,
    /// - OrderDateDescending = 2,
    /// - PaidDateAscending = 3,
    /// - PaidDateDescending = 4,
    /// - CancelledDateAscending = 5,
    /// - CancelledDateDescending = 6,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1")]
    [Authorize]
    public async Task<ActionResult<BasePaginationResponse<GetOrderResponse>>>
        GetOrders(RequestOptionsBase<GetOrderFilterOption, GetOrderSortOption> request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);

        var paginationResponse = await _orderService.GetOrders(userId, request);

        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetOrderResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get orders successfully";
        
        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Lấy chi tiết đơn hàng, yêu cầu đăng nhập
    /// </summary>
    /// <param name="orderId">Lấy từ request path</param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/detail/{orderId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<GetDetailsOfOrderResponse>>> GetOrderDetail(Guid orderId)
    {
        var orderDetail = await _orderService.GetOrderDetail(orderId);

        return Ok(new BaseResponse(StatusCodes.Status200OK, "Get order detail successfully", orderDetail));
    }
    
    /// <summary>
    /// Lấy danh sách đơn hàng của tất cả khách hàng, yêu cầu đăng nhập với quyền ADMIN
    /// </summary>
    /// <remarks>
    /// orderType:
    ///
    /// - EWalletDeposit = 1,
    /// - MasterTemplates = 2,
    /// - RecordFiles = 3,
    /// - Poems = 4,
    ///
    /// orderStatus:
    ///
    /// - Pending = 1,
    /// - Paid = 2,
    /// - Cancelled = 3
    /// 
    /// SortOptions:
    ///
    /// - OrderDateAscending = 1,
    /// - OrderDateDescending = 2,
    /// - PaidDateAscending = 3,
    /// - PaidDateDescending = 4,
    /// - CancelledDateAscending = 5,
    /// - CancelledDateDescending = 6,
    /// - AmountAscending = 7,
    /// - AmountDescending = 8,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/admin")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BasePaginationResponse<AdminGetOrderResponse>>>
        AdminGetOrders(RequestOptionsBase<AdminGetOrderFilterOption, AdminGetOrderSortOption> request)
    {
        var paginationResponse = await _orderService.AdminGetOrders(request);

        var basePaginationResponse = _mapper.Map<BasePaginationResponse<AdminGetOrderResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get orders successfully";
        
        return Ok(basePaginationResponse);
    }
}
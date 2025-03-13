using AutoMapper;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.OrderFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.OrderSorts;

namespace PoemTown.Service.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<GetOrderResponse>> 
        GetOrders(Guid userId, RequestOptionsBase<GetOrderFilterOption, GetOrderSortOption> request)
    {
        var orderQuery = _unitOfWork.GetRepository<Order>().AsQueryable();

        orderQuery = orderQuery.Where(p => p.UserId == userId);
        
        // Filter
        if (request.FilterOptions != null)
        {
            if (request.FilterOptions.Type != null)
            {
                orderQuery = orderQuery.Where(p => p.Type == request.FilterOptions.Type);
            }
            
            if (request.FilterOptions.Status != null)
            {
                orderQuery = orderQuery.Where(p => p.Status == request.FilterOptions.Status);
            }
        }

        // Sort
        orderQuery = request.SortOptions switch
        {
            GetOrderSortOption.OrderDateAscending => orderQuery.OrderBy(p => p.OrderDate),
            GetOrderSortOption.OrderDateDescending => orderQuery.OrderByDescending(p => p.OrderDate),
            GetOrderSortOption.CancelledDateAscending => orderQuery.OrderBy(p => p.CancelledDate),
            GetOrderSortOption.CancelledDateDescending => orderQuery.OrderByDescending(p => p.CancelledDate),
            GetOrderSortOption.PaidDateAscending => orderQuery.OrderBy(p => p.PaidDate),
            GetOrderSortOption.PaidDateDescending => orderQuery.OrderByDescending(p => p.PaidDate),
            _ => orderQuery.OrderBy(p => p.Type).ThenByDescending(p => p.OrderDate)
        };
        
        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<Order>()
            .GetPagination(orderQuery, request.PageNumber, request.PageSize);
        
        var orders = _mapper.Map<IList<GetOrderResponse>>(queryPaging.Data);

        return new PaginationResponse<GetOrderResponse>(orders, queryPaging.PageNumber,
            queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }
    
    
    public async Task<PaginationResponse<AdminGetOrderResponse>> 
        AdminGetOrders(RequestOptionsBase<AdminGetOrderFilterOption, AdminGetOrderSortOption> request)
    {
        var orderQuery = _unitOfWork.GetRepository<Order>().AsQueryable();
        
        // Filter
        if (request.FilterOptions != null)
        {
            if (request.FilterOptions.Type != null)
            {
                orderQuery = orderQuery.Where(p => p.Type == request.FilterOptions.Type);
            }
            
            if (request.FilterOptions.Status != null)
            {
                orderQuery = orderQuery.Where(p => p.Status == request.FilterOptions.Status);
            }
            
            if (request.FilterOptions.OrderCode != null)
            {
                orderQuery = orderQuery.Where(p => p.OrderCode!.Contains(request.FilterOptions.OrderCode));
            }
            
            if (request.FilterOptions.Email != null)
            {
                orderQuery = orderQuery.Where(p => p.User.Email!.ToLower().Trim()
                    .Contains(request.FilterOptions.Email.ToLower().Trim()));
            }
            
            if (request.FilterOptions.PhoneNumber != null)
            {
                orderQuery = orderQuery.Where(p => p.User.PhoneNumber!.ToLower().Trim()
                    .Contains(request.FilterOptions.PhoneNumber.ToLower().Trim()));
            }
        }

        // Sort
        orderQuery = request.SortOptions switch
        {
            AdminGetOrderSortOption.OrderDateAscending => orderQuery.OrderBy(p => p.OrderDate),
            AdminGetOrderSortOption.OrderDateDescending => orderQuery.OrderByDescending(p => p.OrderDate),
            AdminGetOrderSortOption.CancelledDateAscending => orderQuery.OrderBy(p => p.CancelledDate),
            AdminGetOrderSortOption.CancelledDateDescending => orderQuery.OrderByDescending(p => p.CancelledDate),
            AdminGetOrderSortOption.PaidDateAscending => orderQuery.OrderBy(p => p.PaidDate),
            AdminGetOrderSortOption.PaidDateDescending => orderQuery.OrderByDescending(p => p.PaidDate),
            AdminGetOrderSortOption.AmountAscending => orderQuery.OrderBy(p => p.Amount),
            AdminGetOrderSortOption.AmountDescending => orderQuery.OrderByDescending(p => p.Amount),
            _ => orderQuery.OrderBy(p => p.Type).ThenByDescending(p => p.OrderDate)
        };
        
        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<Order>()
            .GetPagination(orderQuery, request.PageNumber, request.PageSize);
        
        IList<AdminGetOrderResponse> orders = new List<AdminGetOrderResponse>();
        
        foreach (var order in queryPaging.Data)
        {
            var orderEntity = await _unitOfWork.GetRepository<Order>().FindAsync(p => p.Id == order.Id);

            // Check if order is null
            if (orderEntity == null)
            {
                continue;
            }
            
            orders.Add(_mapper.Map<AdminGetOrderResponse>(orderEntity));
            
            // Map user in order
            orders.Last().User = _mapper.Map<GetUserInOrderResponse>(orderEntity.User);
        }        

        return new PaginationResponse<AdminGetOrderResponse>(orders, queryPaging.PageNumber,
            queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }
    
    public async Task<GetDetailsOfOrderResponse> GetOrderDetail(Guid orderId)
    {
        var order = await _unitOfWork.GetRepository<Order>().FindAsync(p => p.Id == orderId);
        
        // Check if order is null
        if (order == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Order not found");
        }
        
        var orderDetails = _unitOfWork.GetRepository<OrderDetail>().AsQueryable()
            .Where(p => p.OrderId == orderId).ToList();

        var response = _mapper.Map<GetDetailsOfOrderResponse>(order);
        
        // Map details of order
        response.OrderDetails = _mapper.Map<IList<GetOrderDetailResponse>>(orderDetails);

        return response;
    }
}
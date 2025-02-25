using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.PaymentGatewayRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PaymentGatewayResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.PaymentGatewayFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PaymentGatewaySorts;

namespace PoemTown.Service.Services;

public class PaymentGatewayService : IPaymentGatewayService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public PaymentGatewayService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<PaginationResponse<GetPaymentGatewayResponse>>
        GetPaymentGateways(RequestOptionsBase<GetPaymentGatewayFilterOptions, GetPaymentGatewaySortOptions> request)
    {
        var paymentGatewayQuery = _unitOfWork.GetRepository<PaymentGateway>().AsQueryable();

        // IsDelete
        if (request.IsDelete == true)
        {
            paymentGatewayQuery = paymentGatewayQuery.Where(x => x.DeletedTime != null);
        }
        else
        {
            paymentGatewayQuery = paymentGatewayQuery.Where(x => x.DeletedTime == null);
        }

        // Filter
        if (request.FilterOptions != null)
        {
            if (request.FilterOptions.IsSuspended != null)
            {
                paymentGatewayQuery = paymentGatewayQuery.Where(p => p.IsSuspended == request.FilterOptions.IsSuspended);
            }
        }

        // Sort
        paymentGatewayQuery = request.SortOptions switch
        {
            GetPaymentGatewaySortOptions.CreatedTimeAscending => paymentGatewayQuery.OrderBy(x => x.CreatedTime),
            GetPaymentGatewaySortOptions.CreatedTimeDescending => paymentGatewayQuery.OrderByDescending(x =>
                x.CreatedTime),
            _ => paymentGatewayQuery.OrderByDescending(x => x.CreatedTime)
        };

        // Pagination
        var queryPagination = await _unitOfWork.GetRepository<PaymentGateway>()
            .GetPagination(paymentGatewayQuery, request.PageNumber, request.PageSize);
        
        var paymentGateways = _mapper.Map<List<GetPaymentGatewayResponse>>(queryPagination.Data);
        
        return new PaginationResponse<GetPaymentGatewayResponse>(paymentGateways, queryPagination.PageNumber,
            queryPagination.PageSize, queryPagination.TotalRecords, queryPagination.CurrentPageRecords);
    }

    public async Task CreatePaymentGateway(CreatePaymentGatewayRequest request)
    {
        // Format string input
        request.Name = StringHelper.FormatStringInput(request.Name);
        var paymentGateway = _mapper.Map<PaymentGateway>(request);

        await _unitOfWork.GetRepository<PaymentGateway>().InsertAsync(paymentGateway);
        await _unitOfWork.SaveChangesAsync();
    }
}
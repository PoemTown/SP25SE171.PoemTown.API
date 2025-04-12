using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.PaymentGatewayRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PaymentGatewayResponses;
using PoemTown.Service.BusinessModels.ResponseModels.TransactionResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class PaymentGatewayMapping : Profile
{
    public PaymentGatewayMapping()
    {
        CreateMap<CreatePaymentGatewayRequest, PaymentGateway>();
        CreateMap<PaymentGateway, GetPaymentGatewayResponse>().ReverseMap();
        CreateMap<UpdatePaymentGatewayRequest, PaymentGateway>();
        CreateMap<GetPaymentGatewayResponse, GetTransactionDetailResponse>();
    }
}
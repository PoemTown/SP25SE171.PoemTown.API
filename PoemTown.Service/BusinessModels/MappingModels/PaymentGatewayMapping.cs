using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.PaymentGatewayRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PaymentGatewayResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class PaymentGatewayMapping : Profile
{
    public PaymentGatewayMapping()
    {
        CreateMap<CreatePaymentGatewayRequest, PaymentGateway>();
        CreateMap<PaymentGateway, GetPaymentGatewayResponse>().ReverseMap();
    }
}
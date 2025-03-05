using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class OrderMapping : Profile
{
    public OrderMapping()
    {
        CreateMap<Order, GetOrderResponse>().ReverseMap();
        CreateMap<Order, GetDetailsOfOrderResponse>().ReverseMap();
        CreateMap<OrderDetail, GetOrderDetailResponse>().ReverseMap();

        CreateMap<GetPoemInOrderDetailResponse, GetOrderDetailResponse>();
        CreateMap<GetRecordFileInOrderDetailResponse, GetOrderDetailResponse>();
        CreateMap<GetMasterTemplateInOrderDetailResponse, GetOrderDetailResponse>();
    }
}
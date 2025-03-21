using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class SaleVersionMapping : Profile
{
    public SaleVersionMapping()
    {
        CreateMap<SaleVersion, GetSaleVersionInOrderDetailResponse>().ReverseMap();
    }
}
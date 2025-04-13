using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.SaleVersionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UsageResponse;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class SaleVersionMapping : Profile
{
    public SaleVersionMapping()
    {
        CreateMap<SaleVersion, GetSaleVersionInOrderDetailResponse>().ReverseMap();
        CreateMap<SaleVersion, GetSaleVersionResponse>().ReverseMap();
        CreateMap<SaleVersion, GetPoemVersionResponse>().ReverseMap();

        
        CreateMap<GetSaleVersionResponse, GetPoemDetailResponse>();
        CreateMap<GetSaleVersionResponse, GetPoemInCollectionResponse>();
        CreateMap<GetSaleVersionResponse, GetPoemInTargetMarkResponse>();
        CreateMap<GetSaleVersionResponse, GetPoemResponse>();
        CreateMap<GetSaleVersionResponse, GetPostedPoemResponse>();
    }
}
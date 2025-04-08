using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.PoetSampleRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PoetSampleResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class PoetSampleMapping : Profile
{
    public PoetSampleMapping()
    {
        CreateMap<CreateNewPoetSampleRequest, PoetSample>();
        CreateMap<UpdatePoetSampleRequest, PoetSample>();

        CreateMap<PoetSample, GetPoetSampleResponse>().ReverseMap();
        
    }
}
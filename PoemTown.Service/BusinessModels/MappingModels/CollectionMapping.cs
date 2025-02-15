using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.CollectionRequest;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class CollectionMapping : Profile
{
    public CollectionMapping()
    {
        CreateMap<CreateCollectionRequest, Collection>();
        CreateMap<UpdateCollectionRequest, Collection>();
        CreateMap<GetCollectionResponse, Collection>().ReverseMap();

    }
}
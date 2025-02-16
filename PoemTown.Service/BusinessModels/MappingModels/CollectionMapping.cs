using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.CollectionRequest;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;


namespace PoemTown.Service.BusinessModels.MappingModels;

public class CollectionMapping : Profile
{
    public CollectionMapping()
    {
        CreateMap<Collection, GetCollectionInTargetMarkResponse>().ReverseMap();
        CreateMap<Collection, GetCollectionInPoemResponse>().ReverseMap();
        
        CreateMap<CreateCollectionRequest, Collection>();
        CreateMap<UpdateCollectionRequest, Collection>();
        CreateMap<GetCollectionResponse, Collection>().ReverseMap();
    }
}
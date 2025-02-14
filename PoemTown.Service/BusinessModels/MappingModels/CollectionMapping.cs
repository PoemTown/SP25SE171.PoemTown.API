using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class CollectionMapping : Profile
{
    public CollectionMapping()
    {
        CreateMap<Collection, GetCollectionInTargetMarkResponse>().ReverseMap();
    }
}
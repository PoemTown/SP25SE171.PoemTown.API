using AutoMapper;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class TargetMarkMapping : Profile
{
    public TargetMarkMapping()
    {
        CreateMap<GetBasicUserInformationResponse, GetPoemInTargetMarkResponse>();
        CreateMap<GetBasicUserInformationResponse, GetCollectionInTargetMarkResponse>();
    }
}
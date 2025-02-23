using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.TargetMarkResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class TargetMarkMapping : Profile
{
    public TargetMarkMapping()
    {
        CreateMap<GetBasicUserInformationResponse, GetPoemInTargetMarkResponse>();
        CreateMap<GetBasicUserInformationResponse, GetCollectionInTargetMarkResponse>();

        CreateMap<TargetMark, GetTargetMarkResponse>().ReverseMap();
    }
}
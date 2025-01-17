using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class PoemMapping : Profile
{
    public PoemMapping()
    {
        CreateMap<CreateNewPoemRequest, Poem>();
        CreateMap<Poem, GetPoemResponse>().ReverseMap();
    }
}
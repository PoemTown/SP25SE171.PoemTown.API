using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class PoemMapping : Profile
{
    public PoemMapping()
    {
        CreateMap<CreateNewPoemRequest, Poem>();
    }
}
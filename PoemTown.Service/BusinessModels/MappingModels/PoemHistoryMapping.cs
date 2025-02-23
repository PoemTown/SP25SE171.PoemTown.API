using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.PoemHistoryResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class PoemHistoryMapping : Profile
{
    public PoemHistoryMapping()
    {
        //Mapping from Poem to PoemHistory
        CreateMap<Poem, PoemHistory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<PoemHistory, GetPoemHistoryResponse>().ReverseMap();
        CreateMap<PoemHistory, GetPoemHistoryDetailResponse>().ReverseMap();
    }
}
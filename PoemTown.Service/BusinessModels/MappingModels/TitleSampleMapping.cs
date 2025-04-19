using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.TitleSampleResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class TitleSampleMapping : Profile
{
    public TitleSampleMapping()
    {
        CreateMap<TitleSample, GetTitleSampleResponse>().ReverseMap();
    }
}
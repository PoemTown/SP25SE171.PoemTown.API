using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.LikeResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class LikeMapping : Profile
{
    public LikeMapping()
    {
        CreateMap<Like, GetLikeResponse>().ReverseMap();
        
        
    }
}
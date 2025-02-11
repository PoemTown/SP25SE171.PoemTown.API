using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PoemHistoryResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class PoemMapping : Profile
{
    public PoemMapping()
    {
        CreateMap<CreateNewPoemRequest, Poem>();
        CreateMap<UpdatePoemRequest, Poem>();
        
        CreateMap<GetPoemResponse, Poem>().ReverseMap()
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(p => p.Likes!.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(p => p.Comments!.Count));
        
        CreateMap<GetPoemHistoryResponse, GetPoemResponse>();
    }
}
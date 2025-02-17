using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemHistoryResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;
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
        
        CreateMap<GetRecordFileResponse, GetPoemDetailResponse>().ReverseMap();
        CreateMap<GetPoemDetailResponse, Poem>().ReverseMap()
            .ForMember(dest => dest.RecordFiles, opt => opt.Ignore())
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(p => p.Likes!.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(p => p.Comments!.Count));
        
        CreateMap<GetPoemInCollectionResponse, Poem>().ReverseMap()
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(p => p.Likes!.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(p => p.Comments!.Count));
        CreateMap<GetPostedPoemResponse, Poem>().ReverseMap()
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(p => p.Likes!.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(p => p.Comments!.Count));
        
        // poem in target mark
        CreateMap<GetPoemInTargetMarkResponse, Poem>().ReverseMap()
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(p => p.Likes!.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(p => p.Comments!.Count));

        // poem in collection
        CreateMap<GetCollectionInPoemResponse, GetPoemResponse>();
        CreateMap<GetCollectionInPoemResponse, GetPoemDetailResponse>();
        CreateMap<GetCollectionInPoemResponse, GetPoemResponse>();
    }
}
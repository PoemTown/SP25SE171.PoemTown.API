using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.CollectionRequest;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.TargetMarkResponses;


namespace PoemTown.Service.BusinessModels.MappingModels;

public class CollectionMapping : Profile
{
    public CollectionMapping()
    {
        CreateMap<GetCollectionInTargetMarkResponse, Collection>().ReverseMap()
            .ForMember(dest => dest.TotalChapter, opt => opt.MapFrom(p => p.Poems!.Count))
            .ForMember(dest => dest.TotalRecord, opt => opt.MapFrom(p => p.Poems.SelectMany(p => p.RecordFiles.Where(r => r.DeletedTime == null)).Count()));;
        
        CreateMap<Collection, GetCollectionInPoemResponse>().ReverseMap();
        
        CreateMap<CreateCollectionRequest, Collection>();
        CreateMap<UpdateCollectionRequest, Collection>();
        CreateMap<GetPoetSampleCollectionResponse, Collection>().ReverseMap()
            .ForMember(dest => dest.TotalChapter, opt => opt.MapFrom(p => p.Poems!.Count))
            .ForMember(dest => dest.TotalRecord, opt => opt.MapFrom(p => p.Poems.SelectMany(p => p.RecordFiles.Where(r => r.DeletedTime == null)).Count()));
        
        CreateMap<GetCollectionResponse, Collection>().ReverseMap()
             .ForMember(dest => dest.TotalChapter, opt => opt.MapFrom(p => p.Poems!.Count))
             .ForMember(dest => dest.TotalRecord, opt => opt.MapFrom(p => p.Poems.SelectMany(p => p.RecordFiles.Where(r => r.DeletedTime == null)).Count()));


        CreateMap<GetUserCollectionResponse, Collection>().ReverseMap()
            .ForMember(dest => dest.TotalChapter, opt => opt.MapFrom(p => p.Poems!.Count))
            .ForMember(dest => dest.TotalRecord, opt => opt.MapFrom(p => p.Poems.SelectMany(p => p.RecordFiles.Where(r => r.DeletedTime == null)).Count()));
    }
}
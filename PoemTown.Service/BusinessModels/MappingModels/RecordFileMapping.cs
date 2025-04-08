using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.RequestModels.RecordFileRequests;
using PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;
using PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class RecordFileMapping : Profile
{
    public RecordFileMapping()
    {
        CreateMap<CreateNewRecordFileRequest, RecordFile>();
        
        CreateMap<CreateNewRecordFileRequest, CreateNewPoemRequest>();
        CreateMap<UpdateRecordRequest, RecordFile>();

        CreateMap<RecordFile, GetBoughtRecordResponse>()
       .ForMember(dest => dest.Poem, opt => opt.MapFrom(p => p.Poem));

        CreateMap<RecordFile, GetSoldRecordResponse>()
       .ForMember(dest => dest.Poem, opt => opt.MapFrom(p => p.Poem));


        CreateMap<RecordFile, GetRecordFileResponse>().ReverseMap()
            .ForMember(dest => dest.Poem, opt => opt.MapFrom(p => p.Poem));

        CreateMap<RecordFile, GetRecordFileResponse>()
            .ForMember(dest => dest.Poem, opt => opt.MapFrom(p => p.Poem))
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.User));


        CreateMap<RecordFile, GetRecordFileInOrderDetailResponse>().ReverseMap();
    }
}
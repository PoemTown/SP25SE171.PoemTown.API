using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.PoemTypeRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PoemHistoryResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemTypeResponses;
using PoemTown.Service.BusinessModels.ResponseModels.ReportResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class PoemTypeMapping : Profile
{
    public PoemTypeMapping()
    {
        CreateMap<PoemType, GetPoemTypeResponse>().ReverseMap();
        CreateMap<CreatePoemTypeRequest, PoemType>();
        CreateMap<UpdatePoemTypeRequest, PoemType>();

        CreateMap<GetPoemTypeResponse, GetPoemDetailResponse>();
        CreateMap<GetPoemTypeResponse, GetPoemResponse>();
        CreateMap<GetPoemTypeResponse, GetPostedPoemResponse>();
        CreateMap<GetPoemTypeResponse, GetPoemInCollectionResponse>();
        CreateMap<GetPoemTypeResponse, GetPoetSamplePoemResponse>();
        CreateMap<GetPoemTypeResponse, GetPoemInReportResponse>();
        CreateMap<GetPoemTypeResponse, GetPoemHistoryResponse>();
        CreateMap<GetPoemTypeResponse, GetPoemInTargetMarkResponse>();
        CreateMap<GetPoemTypeResponse, PoemPlagiarismFromResponse>();
        CreateMap<GetPoemTypeResponse, GetPoemHistoryDetailResponse>();
    }
}
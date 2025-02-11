using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.RequestModels.RecordFileRequests;
using PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class RecordFileMapping : Profile
{
    public RecordFileMapping()
    {
        CreateMap<CreateNewRecordFileRequest, RecordFile>();
        
        CreateMap<CreateNewRecordFileRequest, CreateNewPoemRequest>();

        CreateMap<RecordFile, GetRecordFileResponse>().ReverseMap();
    }
}
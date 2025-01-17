using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.RequestModels.RecordFileRequests;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class RecordFileMapping : Profile
{
    public RecordFileMapping()
    {
        CreateMap<CreateNewRecordFileRequest, RecordFile>();
        
        CreateMap<CreateNewRecordFileRequest, CreateNewPoemRequest>();
    }
}
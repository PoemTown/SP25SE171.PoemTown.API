using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.DailyMessageRequests;
using PoemTown.Service.BusinessModels.ResponseModels.DailyMessageResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class DailyMessageMapping : Profile
{
    public DailyMessageMapping()
    {
        CreateMap<CreateNewDailyMessageRequest, DailyMessage>();
        CreateMap<UpdateDailyMessageRequest, DailyMessage>();
        CreateMap<DailyMessage, GetDailyMessageResponse>().ReverseMap();
    }
}
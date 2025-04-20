using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.SystemContactRequests;
using PoemTown.Service.BusinessModels.ResponseModels.SystemContactResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class SystemContactMapping : Profile
{
    public SystemContactMapping()
    {
        CreateMap<CreateNewSystemContactRequest, SystemContact>();
        CreateMap<UpdateSystemContactRequest, SystemContact>();
        CreateMap<SystemContact, GetSystemContactResponse>().ReverseMap();
    }
}
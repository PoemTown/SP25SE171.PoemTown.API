using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.AnnouncementResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class AnnouncementMapping : Profile
{
    public AnnouncementMapping()
    {
        CreateMap<Announcement, GetAnnouncementResponse>().ReverseMap();
    }
}
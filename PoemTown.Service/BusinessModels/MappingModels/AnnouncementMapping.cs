using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.AnnouncementResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class AnnouncementMapping : Profile
{
    public AnnouncementMapping()
    {
        CreateMap<GetAnnouncementResponse, Announcement>().ReverseMap()
            .ForMember(dest => dest.FollowerUserId,
                opt => opt.MapFrom(src => src.Follower != null ? src.Follower.FollowUserId : null));
    }
}
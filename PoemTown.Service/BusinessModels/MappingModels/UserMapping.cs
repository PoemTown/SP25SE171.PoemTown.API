using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.UserRequests;
using PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;
using PoemTown.Service.BusinessModels.ResponseModels.ReportResponses;
using PoemTown.Service.BusinessModels.ResponseModels.TransactionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, GetBasicUserInformationResponse>().ReverseMap();
        CreateMap<User, GetUserProfileResponse>()
            .ForMember(desc => desc.TotalFollower, opt => opt.MapFrom(src => src.FollowedUser.Count))
            .ReverseMap();
        CreateMap<User, GetUserInTransactionResponse>().ReverseMap();
        CreateMap<User, GetUserInOrderResponse>().ReverseMap();
        CreateMap<UpdateMyProfileRequest, User>();
        CreateMap<User, GetReportUserInReportResponse>().ReverseMap();
        CreateMap<User, GetOwnOnlineProfileResponse>()
            .ForMember(dest => dest.TotalFollowers, opt => opt.MapFrom(src => src.FollowedUser.Count))
            .ForMember(dest => dest.TotalFollowings, opt => opt.MapFrom(src => src.FollowUser.Count));

        CreateMap<User, GetUserOnlineProfileResponse>().ReverseMap();
    }
}
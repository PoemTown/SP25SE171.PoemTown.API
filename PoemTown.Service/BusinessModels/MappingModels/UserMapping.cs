using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.UserRequests;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, GetBasicUserInformationResponse>().ReverseMap();
        CreateMap<User, GetUserProfileResponse>().ReverseMap();
        CreateMap<UpdateMyProfileRequest, User>();
    }
}
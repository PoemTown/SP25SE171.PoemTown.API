using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.FollowerResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class FollowerMapping : Profile
{
    public FollowerMapping()
    {
        CreateMap<Follower, GetFollowersResponse>().ReverseMap();
        
        // Map user information to the GetMyFollowerResponse
        CreateMap<GetBasicUserInformationResponse, GetFollowersResponse>();
    }
}
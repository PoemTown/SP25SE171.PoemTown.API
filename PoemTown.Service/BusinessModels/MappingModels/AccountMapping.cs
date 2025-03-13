using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.AccountResponses;
using PoemTown.Service.BusinessModels.ResponseModels.RoleResponse;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class AccountMapping : Profile
{
    public AccountMapping()
    {
        CreateMap<User, GetAccountResponse>().ReverseMap();
        CreateMap<User, GetAccountDetailResponse>().ReverseMap();
    }
}
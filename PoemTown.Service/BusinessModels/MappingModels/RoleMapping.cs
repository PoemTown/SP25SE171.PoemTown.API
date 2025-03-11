using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.RoleResponse;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class RoleMapping : Profile
{
    public RoleMapping()
    {
        CreateMap<Role, GetRoleResponse>();
    }
}
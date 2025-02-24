
using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.ThemeRequests;
using PoemTown.Service.BusinessModels.ResponseModels.ThemeResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class ThemeMapping : Profile
{
    public ThemeMapping()
    {
        CreateMap<CreateUserThemeRequest, Theme>();
        CreateMap<Theme, GetThemeResponse>().ReverseMap();
        CreateMap<UpdateUserThemeRequest, Theme>();
    }
}
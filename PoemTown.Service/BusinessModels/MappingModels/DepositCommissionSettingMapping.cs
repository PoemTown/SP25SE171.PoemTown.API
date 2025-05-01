using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.DepositCommissionSettingResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class DepositCommissionSettingMapping : Profile
{
    public DepositCommissionSettingMapping()
    {
        CreateMap<DepositCommissionSetting, GetDepositCommissionSettingsResponse>().ReverseMap();
    }
}
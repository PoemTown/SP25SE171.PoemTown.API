using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.BankTypeRequests;
using PoemTown.Service.BusinessModels.ResponseModels.BankTypeResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class BankTypeMapping : Profile
{
    public BankTypeMapping()
    {
        CreateMap<CreateNewBankTypeRequest, BankType>();
        CreateMap<UpdateBankTypeRequest, BankType>();
        CreateMap<BankType, GetBankTypeResponse>().ReverseMap();

        CreateMap<CreateUserBankTypeRequest, UserBankType>();
        CreateMap<UpdateUserBankTypeRequest, UserBankType>();
        CreateMap<GetUserBankTypeResponse, UserBankType>().ReverseMap();
    }
}
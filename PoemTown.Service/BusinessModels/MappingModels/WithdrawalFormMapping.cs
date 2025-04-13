using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.WithdrawalFormRequests;
using PoemTown.Service.BusinessModels.ResponseModels.WithdrawalFormResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class WithdrawalFormMapping : Profile
{
    public WithdrawalFormMapping()
    {
        CreateMap<CreateWithdrawalFormRequest, WithdrawalForm>();

        CreateMap<GetWithdrawalFormResponse, WithdrawalForm>().ReverseMap();
    }
}
using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.DepositCommissionSettingResponses;
using PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;
using PoemTown.Service.BusinessModels.ResponseModels.TransactionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.WithdrawalFormResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class TransactionMapping : Profile
{
    public TransactionMapping()
    {
        CreateMap<Transaction, UserGetTransactionResponse>().ReverseMap();
        CreateMap<Transaction, GetTransactionResponse>().ReverseMap();
        CreateMap<Transaction, GetTransactionDetailResponse>().ReverseMap();

        CreateMap<GetOrderWithOrderDetailResponse, GetTransactionDetailResponse>();
        CreateMap<GetDepositCommissionSettingsResponse, GetTransactionDetailResponse>();
        CreateMap<GetWithdrawalFormResponse, GetTransactionDetailResponse>();
        
        CreateMap<Transaction, UserGetTransactionDetailResponse>().ReverseMap();

        CreateMap<GetOrderWithOrderDetailResponse, UserGetTransactionDetailResponse>();
        CreateMap<GetDepositCommissionSettingsResponse, UserGetTransactionDetailResponse>();
        CreateMap<GetWithdrawalFormResponse, UserGetTransactionDetailResponse>();
    }
}
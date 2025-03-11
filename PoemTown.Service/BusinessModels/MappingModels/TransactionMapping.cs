using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.TransactionResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class TransactionMapping : Profile
{
    public TransactionMapping()
    {
        CreateMap<Transaction, UserGetTransactionResponse>().ReverseMap();
        CreateMap<Transaction, GetTransactionResponse>().ReverseMap();
        CreateMap<Transaction, GetTransactionDetailResponse>().ReverseMap();
    }
}
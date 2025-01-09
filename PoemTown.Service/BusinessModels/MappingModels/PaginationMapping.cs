using AutoMapper;
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class PaginationMapping : Profile
{
    public PaginationMapping()
    {
        CreateMap(typeof(PaginationResponse<>), typeof(BasePaginationResponse<>));
    }
}
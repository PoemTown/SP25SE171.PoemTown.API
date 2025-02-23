using AutoMapper;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Services;

public class StatisticService : IStatisticService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    } 

    
}
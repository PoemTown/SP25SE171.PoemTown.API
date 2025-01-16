using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Services;

public class PoemService : IPoemService
{
    private readonly IUnitOfWork _unitOfWork;
    public PoemService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /*
    public async Task CreateNewPoem(CreateNewPoemRequest request) 
    {
        if(request.CollectionId )
    }*/
}
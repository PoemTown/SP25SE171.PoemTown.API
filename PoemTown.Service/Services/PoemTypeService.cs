using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.PoemTypeRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PoemTypeResponses;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Services;

public class PoemTypeService : IPoemTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PoemTypeService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<GetPoemTypeResponse>> GetAllPoemTypes()
    {
        var poemTypes = await _unitOfWork.GetRepository<PoemType>()
            .AsQueryable()
            .ToListAsync();
        return _mapper.Map<IEnumerable<GetPoemTypeResponse>>(poemTypes);
    }
    
    public async Task UpdatePoemType(UpdatePoemTypeRequest request)
    {
        var poemType = await _unitOfWork.GetRepository<PoemType>()
            .FindAsync(x => x.Id == request.Id);
        
        // Check if the poem type exists
        if (poemType == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem type not found");
        }
        
        _mapper.Map(request, poemType);
        _unitOfWork.GetRepository<PoemType>().Update(poemType);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<GetPoemTypeResponse> GetPoemTypeById(Guid poemTypeId)
    {
        var poemType = await _unitOfWork.GetRepository<PoemType>()
            .FindAsync(x => x.Id == poemTypeId);
        
        // Check if the poem type exists
        if (poemType == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem type not found");
        }
        
        return _mapper.Map<GetPoemTypeResponse>(poemType);
    }
    
    public async Task CreatePoemType(CreatePoemTypeRequest request)
    {
        var poemType = _mapper.Map<PoemType>(request);
        await _unitOfWork.GetRepository<PoemType>().InsertAsync(poemType);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeletePoemType(Guid poemTypeId)
    {
        var poemType = await _unitOfWork.GetRepository<PoemType>()
            .FindAsync(x => x.Id == poemTypeId);
        
        // Check if the poem type exists
        if (poemType == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem type not found");
        }
        
        // Check if the poem type is in use
        var isInUse = await _unitOfWork.GetRepository<Poem>()
            .AsQueryable()
            .AnyAsync(x => x.PoemTypeId == poemTypeId);
        if (isInUse)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem type is in use and cannot be deleted");
        }
        
        _unitOfWork.GetRepository<PoemType>().Delete(poemType);
        await _unitOfWork.SaveChangesAsync();
    }
}

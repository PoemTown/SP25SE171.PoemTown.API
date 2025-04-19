using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.TitleSampleRequests;
using PoemTown.Service.BusinessModels.ResponseModels.TitleSampleResponses;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Services;

public class TitleSampleService : ITitleSampleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public TitleSampleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    // Create TitleSample
    public async Task CreateTitleSample(CreateTitleSampleRequest request)
    {
        var titleSample = new TitleSample()
        {
            Name = request.Name
        };
        
        await _unitOfWork.GetRepository<TitleSample>().InsertAsync(titleSample);
        await _unitOfWork.SaveChangesAsync();
    }
    
    // Update TitleSample
    public async Task UpdateTitleSample(UpdateTitleSampleRequest request)
    {
        var titleSample = await _unitOfWork.GetRepository<TitleSample>().FindAsync(p => p.Id == request.Id);
        
        // Check if the TitleSample exists
        if (titleSample == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "TitleSample not found");
        }
        
        titleSample.Name = request.Name;
        
        _unitOfWork.GetRepository<TitleSample>().Update(titleSample);
        await _unitOfWork.SaveChangesAsync();
    }
    
    // Delete TitleSample
    public async Task DeleteTitleSample(Guid id)
    {
        var titleSample = await _unitOfWork.GetRepository<TitleSample>().FindAsync(p => p.Id == id);
        
        // Check if the TitleSample exists
        if (titleSample == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "TitleSample not found");
        }
        
        // Check if the TitleSample is associated with any PoetSampleTitleSamples
        if(titleSample.PoetSampleTitleSamples != null && titleSample.PoetSampleTitleSamples.Count > 0)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "TitleSample cannot be deleted because it is associated with PoetSampleTitleSamples");
        }
        
        _unitOfWork.GetRepository<TitleSample>().Delete(titleSample);
        await _unitOfWork.SaveChangesAsync();
    }
    
    // Delete TitleSample permanently
    public async Task DeleteTitleSamplePermanently(Guid id)
    {
        var titleSample = await _unitOfWork.GetRepository<TitleSample>().FindAsync(p => p.Id == id);
        
        // Check if the TitleSample exists
        if (titleSample == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "TitleSample not found");
        }

        // Check if the TitleSample is soft deleted
        if (titleSample.DeletedTime == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "TitleSample has not yet soft deleted");
        }
        
        // Check if the TitleSample is associated with any PoetSampleTitleSamples
        if(titleSample.PoetSampleTitleSamples != null && titleSample.PoetSampleTitleSamples.Count > 0)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "TitleSample cannot be deleted because it is associated with PoetSampleTitleSamples");
        }
        
        _unitOfWork.GetRepository<TitleSample>().DeletePermanent(titleSample);
        await _unitOfWork.SaveChangesAsync();
    }
    
    // Get TitleSample by Id
    public async Task<GetTitleSampleResponse> GetTitleSampleById(Guid id)
    {
        var titleSample = await _unitOfWork.GetRepository<TitleSample>().FindAsync(p => p.Id == id);
        
        // Check if the TitleSample exists
        if (titleSample == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "TitleSample not found");
        }
        
        return _mapper.Map<GetTitleSampleResponse>(titleSample);
    }
    
    // Get all TitleSamples
    public async Task<IEnumerable<GetTitleSampleResponse>> GetAllTitleSamples()
    {
        var titleSamples = await _unitOfWork.GetRepository<TitleSample>()
            .AsQueryable()
            .Where(p => p.DeletedTime == null)
            .ToListAsync();
        
        return _mapper.Map<IEnumerable<GetTitleSampleResponse>>(titleSamples);
    }
}
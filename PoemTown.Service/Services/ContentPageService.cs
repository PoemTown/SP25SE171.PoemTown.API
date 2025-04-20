using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.ContentPageRequests;
using PoemTown.Service.BusinessModels.ResponseModels.ContenPageResponses;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Services;

public class ContentPageService : IContentPageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public ContentPageService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task CreateNewContentPage(CreateNewContentPageRequest request)
    {
        var contentPage = _mapper.Map<ContentPage>(request);
        await _unitOfWork.GetRepository<ContentPage>().InsertAsync(contentPage);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateContentPage(UpdateContentPageRequest request)
    {
        ContentPage? contentPage = await _unitOfWork.GetRepository<ContentPage>().FindAsync(p => p.Id == request.Id);
        
        // Check if the content page exists
        if (contentPage == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Content page not found");
        }
        
        // Update the content page properties
        _mapper.Map(request, contentPage);
        
        _unitOfWork.GetRepository<ContentPage>().Update(contentPage);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteContentPage(Guid contentPageId)
    {
        ContentPage? contentPage = await _unitOfWork.GetRepository<ContentPage>().FindAsync(p => p.Id == contentPageId);
        
        // Check if the content page exists
        if (contentPage == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Content page not found");
        }
        
        _unitOfWork.GetRepository<ContentPage>().Delete(contentPage);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteContentPagePermanently(Guid contentPageId)
    {
        ContentPage? contentPage = await _unitOfWork.GetRepository<ContentPage>().FindAsync(p => p.Id == contentPageId);
        
        // Check if the content page exists
        if (contentPage == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Content page not found");
        }

        // Check if the content page has not yet soft deleted
        if (contentPage.DeletedTime == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Content page is not deleted");
        }
        
        _unitOfWork.GetRepository<ContentPage>().DeletePermanent(contentPage);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<GetContentPageResponse> GetContentPage(Guid contentPageId)
    {
        ContentPage? contentPage = await _unitOfWork.GetRepository<ContentPage>().FindAsync(p => p.Id == contentPageId);
        
        // Check if the content page exists
        if (contentPage == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Content page not found");
        }
        
        return _mapper.Map<GetContentPageResponse>(contentPage);
    }
    
    public async Task<IEnumerable<GetContentPageResponse>> GetContentPages()
    {
        var contentPages = await _unitOfWork.GetRepository<ContentPage>()
            .AsQueryable()
            .Where(p => p.DeletedTime == null)
            .ToListAsync();
        
        return _mapper.Map<IEnumerable<GetContentPageResponse>>(contentPages);
    }
}
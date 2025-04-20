using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.SystemContactRequests;
using PoemTown.Service.BusinessModels.ResponseModels.SystemContactResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.ThirdParties.Interfaces;
using PoemTown.Service.ThirdParties.Models.AwsS3;

namespace PoemTown.Service.Services;

public class SystemContactService : ISystemContactService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAwsS3Service _awsS3Service;
    public SystemContactService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IAwsS3Service awsS3Service)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _awsS3Service = awsS3Service;
    }

    public async Task CreateNewSystemContact(CreateNewSystemContactRequest request)
    {
        SystemContact systemContact = _mapper.Map<SystemContact>(request);
        
        await _unitOfWork.GetRepository<SystemContact>().InsertAsync(systemContact);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateSystemContact(UpdateSystemContactRequest request)
    {
        SystemContact? systemContact = await _unitOfWork.GetRepository<SystemContact>().FindAsync(p => p.Id == request.Id);
        
        // Check if the system contact exists
        if (systemContact == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "System contact not found");
        }

        _mapper.Map(request, systemContact);
        
        _unitOfWork.GetRepository<SystemContact>().Update(systemContact);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteSystemContact(Guid id)
    {
        SystemContact? systemContact = await _unitOfWork.GetRepository<SystemContact>().FindAsync(p => p.Id == id);
        
        // Check if the system contact exists
        if (systemContact == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "System contact not found");
        }

        _unitOfWork.GetRepository<SystemContact>().Delete(systemContact);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<GetSystemContactResponse> GetSystemContactById(Guid id)
    {
        SystemContact? systemContact = await _unitOfWork.GetRepository<SystemContact>().FindAsync(p => p.Id == id);
        
        // Check if the system contact exists
        if (systemContact == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "System contact not found");
        }

        return _mapper.Map<GetSystemContactResponse>(systemContact);
    }
    
    public async Task<IEnumerable<GetSystemContactResponse>> GetAllSystemContacts()
    {
        var systemContacts = await _unitOfWork.GetRepository<SystemContact>()
            .AsQueryable()
            .Where(p => p.DeletedTime == null)
            .ToListAsync();
        
        return _mapper.Map<IEnumerable<GetSystemContactResponse>>(systemContacts);
    }
    
    public async Task<string> UploadSystemContactIcon(IFormFile file)
    {
        // Validate the file
        ImageHelper.ValidateImage(file);

        // Upload image to AWS S3
        var fileName = "system-contact-icon";
        UploadImageToAwsS3Model s3Model = new UploadImageToAwsS3Model()
        {
            File = file,
            FolderName = fileName
        };
        return await _awsS3Service.UploadImageToAwsS3Async(s3Model);
    }
}
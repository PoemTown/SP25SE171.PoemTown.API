using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Accounts;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.UserRequests;
using PoemTown.Service.BusinessModels.ResponseModels.TemplateResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.ThirdParties.Interfaces;
using PoemTown.Service.ThirdParties.Models.AwsS3;

namespace PoemTown.Service.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAwsS3Service _awsS3Service;
    private readonly ITemplateService _templateService;
    
    public UserService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IAwsS3Service awsS3Service,
        ITemplateService templateService
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _awsS3Service = awsS3Service;
        _templateService = templateService;
    }

    public async Task<GetUserProfileResponse> GetMyProfile(Guid userId)
    {
        var user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);
        if (user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }

        if (user.Status != AccountStatus.Active)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User is not active");
        }
        
        return _mapper.Map<GetUserProfileResponse>(user);
    }

    public async Task UpdateMyProfile(Guid userId, UpdateMyProfileRequest request)
    {
        var existUserName = await _unitOfWork.GetRepository<User>()
            .AsQueryable()
            .AnyAsync(p => p.UserName!.ToLower().Trim() == request.UserName!.ToLower().Trim() && p.Id != userId);
        
        if(existUserName)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User name already exist");
        }
        
        var user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);
        if (user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }

        if (user.Status != AccountStatus.Active)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User is not active");
        }
        
        _mapper.Map(request, user);
        user.NormalizedUserName = StringHelper.CapitalizeString(user.UserName);
        user.NormalizedEmail = StringHelper.CapitalizeString(user.Email);
        _unitOfWork.GetRepository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<string> UploadProfileImage(Guid userId, IFormFile file)
    {
        var user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);
        if (user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }

        if (user.Status != AccountStatus.Active)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User is not active");
        }
        
        ImageHelper.ValidateImage(file);
        
        //format file name to avoid duplicate file name with userId, unixTimeStamp
        var fileName = $"profiles/{StringHelper.CapitalizeString(userId.ToString())}";
        
        UploadImageToAwsS3Model s3Model = new UploadImageToAwsS3Model()
        {
            File = file,
            Quality = 80,
            FolderName = fileName
        };
        return await _awsS3Service.UploadImageToAwsS3Async(s3Model);
    }

    public async Task<GetOwnOnlineProfileResponse> GetOwnOnlineProfile(Guid userId)
    {
        var user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);
        if (user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }

        if (user.Status != AccountStatus.Active)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User is not active");
        }
        
        
        return _mapper.Map<GetOwnOnlineProfileResponse>(user);
    }

    public async Task<GetUserOnlineProfileResponse> GetUserOnlineProfileResponse(Guid userId)
    {
        var user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);
        
        // Check if user is not found
        if (user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }

        // Check if this user account is inActive, then throw exception
        if (user.Status != AccountStatus.Active)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User is not active");
        }

        // Map user to GetUserOnlineProfileResponse
        GetUserOnlineProfileResponse userOnlineProfileResponse = _mapper.Map<GetUserOnlineProfileResponse>(user);

        // Get user template details
        userOnlineProfileResponse.UserTemplateDetails = await _templateService.GetUserTemplateDetailInOnlineUserProfile(user.Id);
        
        return userOnlineProfileResponse;
    }
}
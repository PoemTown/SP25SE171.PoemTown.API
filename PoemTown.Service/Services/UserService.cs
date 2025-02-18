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
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public UserService(IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
}
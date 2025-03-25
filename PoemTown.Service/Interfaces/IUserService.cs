using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PoemTown.Service.BusinessModels.RequestModels.UserRequests;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.Interfaces;

public interface IUserService
{
    Task<GetUserProfileResponse> GetMyProfile(Guid userId);
    Task UpdateMyProfile(Guid userId, UpdateMyProfileRequest request);
    Task<string> UploadProfileImage(Guid userId, IFormFile file);
    Task<GetOwnOnlineProfileResponse> GetOwnOnlineProfile(Guid userId);
    Task<GetUserOnlineProfileResponse> GetUserOnlineProfileResponse(Guid userId);
}
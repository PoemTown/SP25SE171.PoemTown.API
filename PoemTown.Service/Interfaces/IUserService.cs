using System.Security.Claims;
using PoemTown.Service.BusinessModels.RequestModels.UserRequests;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.Interfaces;

public interface IUserService
{
    Task<GetUserProfileResponse> GetMyProfile(Guid userId);
    Task UpdateMyProfile(Guid userId, UpdateMyProfileRequest request);
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PoemTown.Service.BusinessModels.RequestModels.AuthenticationRequests;
using PoemTown.Service.BusinessModels.ResponseModels.AuthenResponses;

namespace PoemTown.Service.Interfaces
{
   /// <summary>
    /// Provides authentication-related services, including login, registration, and user context retrieval.
    /// </summary>
    public interface IAuthenService
    {
        /// <summary>
        /// Authenticates a user using their email and password, and returns JWT tokens if successful.
        /// </summary>
        /// <param name="request">The login request containing email and password.</param>
        /// <param name="userAgent">The user's browser or device user agent.</param>
        /// <param name="ipAddress">The user's IP address.</param>
        /// <returns>A <see cref="LoginResponse"/> containing access and refresh tokens, along with user roles.</returns>
        /// <exception cref="CoreException">Thrown when the user is not found, email is not confirmed, or login fails.</exception>
        Task<LoginResponse> Login(LoginRequest request, string userAgent, string ipAddress);

        /// <summary>
        /// Registers a new user account with the provided details.
        /// </summary>
        /// <param name="request">The registration request containing user details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="CoreException">Thrown when the user already exists or the role is not found.</exception>
        Task RegisterAccount(RegisterRequest request);

        /// <summary>
        /// Authenticates a user using their Google account ID and email, creating a new account if one does not exist.
        /// </summary>
        /// <param name="googleId">The unique identifier for the user's Google account.</param>
        /// <param name="email">The email associated with the Google account.</param>
        /// <param name="userAgent">The user's browser or device user agent.</param>
        /// <param name="ipAddress">The user's IP address.</param>
        /// <returns>A <see cref="LoginResponse"/> containing access and refresh tokens, along with user roles.</returns>
        /// <exception cref="CoreException">Thrown when a required role is not found.</exception>
        Task<LoginResponse> LoginWithGoogle(string googleId, string email, string userAgent, string ipAddress);

        /// <summary>
        /// Logs out the user by invalidating their tokens for the specified user agent and IP address.
        /// </summary>
        /// <param name="userId">The ID of the user to log out.</param>
        /// <param name="userAgent">The user's browser or device user agent.</param>
        /// <param name="ipAddress">The user's IP address.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="CoreException">Thrown when the user or their token is not found.</exception>
        Task Logout(Guid userId, string userAgent, string ipAddress);

        /// <summary>
        /// Retrieves the user's context from the provided HTTP context.
        /// </summary>
        /// <param name="context">The HTTP context containing user information.</param>
        /// <returns>A <see cref="UserContextResponse"/> containing the user's IP address and user agent.</returns>
        /// <exception cref="CoreException">Thrown when the IP address or user agent is not available.</exception>
        Task<UserContextResponse> GetUserContext(IHttpContextAccessor context);
    }
}

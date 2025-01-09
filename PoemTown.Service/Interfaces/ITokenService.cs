using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.TokenResponses;

namespace PoemTown.Service.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Generate access token for the user when login
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="userAgent">The current browser user using</param>
        /// <param name="ipAddress">The current user's ipAddress</param>
        /// <returns>AccessToken</returns>
        Task<string> GenerateAccessToken(User user, string userAgent, string ipAddress);
        
        /// <summary>
        /// Generate access token and refresh token for the user when login
        /// </summary>
        /// <param name="user">Is the user</param>
        /// <param name="userAgent">The current browser user using</param>
        /// <param name="ipAddress">The current user's ipaddress</param>
        /// <returns><see cref="TokenResponse"/></returns>
        Task<TokenResponse> GenerateTokens(User user, string userAgent, string ipAddress);
        
        /// <summary>
        /// Generate refresh token for the user when login
        /// </summary>
        /// <param name="user">Is the user</param>
        /// <param name="userAgent">The current browser user using</param>
        /// <param name="ipAddress">The current user's ipaddress</param>
        /// <returns>RefreshToken</returns>
        Task<string> GenerateRefreshToken(User user, string userAgent, string ipAddress);
        bool ValidateTokenExpired(string token);
        bool ValidTokenHash(string token, string inputString);
    }
}

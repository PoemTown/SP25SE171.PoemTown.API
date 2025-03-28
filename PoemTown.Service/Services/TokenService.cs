using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.ConfigurationModels.Jwt;
using PoemTown.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PoemTown.Service.BusinessModels.ResponseModels.TokenResponses;

namespace PoemTown.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;
        private readonly IUnitOfWork _unitOfWork;
        public TokenService(UserManager<User> userManager,
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwtSettings = jwtSettings;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Generate access token for the user when login
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="userAgent">The current browser user using</param>
        /// <param name="ipAddress">The current user's ipAddress</param>
        /// <returns>AccessToken</returns>
        public async Task<string> GenerateAccessToken(User user, string userAgent, string ipAddress)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            
            // Get the user token from the database
            var authenToken = await _unitOfWork.GetRepository<UserToken>()
                .FindAsync(p => p.UserId == user.Id 
                                && p.UserAgentHash == HmacSHA256Hasher.Hash(StringHelper.NormalizeString(userAgent))
                                && p.IpAddressHash == HmacSHA256Hasher.Hash(ipAddress));
                
            if(authenToken == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Token not found");
            }
            
            //inputString is a string is the combination of toUpper(userId) + UserAgentHash + IpAddressHash + value(refreshToken)
            var inputString = StringHelper.CapitalizeString(authenToken.UserId.ToString())
                              + authenToken.UserAgentHash
                              + authenToken.IpAddressHash
                              + authenToken.Value;
            
            //hash the inputString with secrets
            string hashJwtToken = HashJwtStringToken(inputString);

            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim("TokenHash", hashJwtToken),
            new Claim("UserName", user.UserName ?? "")
                
        };
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTimeHelper.SystemTimeNow.DateTime.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Generate refresh token for the user when login
        /// </summary>
        /// <param name="user">Is the user</param>
        /// <param name="userAgent">The current browser user using</param>
        /// <param name="ipAddress">The current user's ipaddress</param>
        /// <returns>RefreshToken</returns>
        public async Task<string> GenerateRefreshToken(User user, string userAgent, string ipAddress)
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)); // Random 64-byte token

            // Check if the user already has a token, retrieve it if it exists
            string hashUserAgent = HmacSHA256Hasher.Hash(StringHelper.NormalizeString(userAgent));
            string hashIpAddress = HmacSHA256Hasher.Hash(ipAddress);
            
            var userToken = await _unitOfWork.GetRepository<UserToken>()
                .FindAsync(p => p.UserId.Equals(user.Id) 
                                && p.IpAddressHash == hashIpAddress 
                                && p.UserAgentHash == hashUserAgent);
            
            string expirationTime = TimeStampHelper
                .GenerateUnixTimeStamp(_jwtSettings.RefreshTokenExpirationHours, 0, 0)
                .ToString();

            // If no token exists for the user/device, create a new one
            if (userToken == null)
            {
                userToken = new UserToken()
                {
                    UserId = user.Id,
                    LoginProvider = "PoemTown",
                    Name = "RefreshToken",
                    Value = refreshToken,
                    ExpirationTime = expirationTime,
                    IpAddressHash = hashIpAddress,
                    UserAgentHash = hashUserAgent
                };
                // Add the new token to the repository
                await _unitOfWork.GetRepository<UserToken>().InsertAsync(userToken);
            }
            else
            {
                // Update the existing token with the new refresh token and expiry time
                userToken.Value = refreshToken;
                userToken.ExpirationTime = expirationTime;

                _unitOfWork.GetRepository<UserToken>().Update(userToken);
            }

            // Save changes to the database
            await _unitOfWork.SaveChangesAsync();

            return refreshToken;
        }
        
        /// <summary>
        /// Generate access token and refresh token for the user when login
        /// </summary>
        /// <param name="user">Is the user</param>
        /// <param name="userAgent">The current browser user using</param>
        /// <param name="ipAddress">The current user's ipaddress</param>
        /// <returns>TokenResponse</returns>
        public async Task<TokenResponse> GenerateTokens(User user, string userAgent, string ipAddress)
        {
            var refreshToken = await GenerateRefreshToken(user, userAgent, ipAddress);
            var accessToken = await GenerateAccessToken(user, userAgent, ipAddress);
            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public bool ValidateTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (!tokenHandler.CanReadToken(token))
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Invalid token");
            }
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var expirationClaim = jwtToken.Claims.FirstOrDefault(p => p.Type == JwtRegisteredClaimNames.Exp);
            if (expirationClaim == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Token does not contain an expiration claim");
            }

            var expirationTimeStamp = long.Parse(expirationClaim.Value);

            if (String.CompareOrdinal(expirationTimeStamp.ToString(), TimeStampHelper.GenerateUnixTimeStampNow().ToString()) <= 0)
            {
                return false;
            }
            return true;
        }

        public string HashJwtStringToken(string inputString)
        {
            var tokenString = _jwtSettings.Key + _jwtSettings.Issuer + _jwtSettings.Audience + inputString;
            // Hash the token string with hmacsha256, using the secret key
            var tokenHasher = HmacSHA256Hasher.Hash(tokenString);
            return tokenHasher;
        }

        public bool ValidTokenHash(string token, string inputString)
        {
            string tokenHash = HashJwtStringToken(inputString);
            return tokenHash == token;
        }

        public async Task RemoveUserRefreshToken(Guid userId)
        {
            IEnumerable<UserToken> userTokens = await _unitOfWork.GetRepository<UserToken>()
                .AsQueryable()
                .Where(p => p.UserId == userId)
                .ToListAsync();
         
            // Delete all user's refresh tokens
            _unitOfWork.GetRepository<UserToken>().DeletePermanentRange(userTokens);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

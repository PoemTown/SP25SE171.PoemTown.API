using System.IdentityModel.Tokens.Jwt;
using System.Net;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.Interfaces;

namespace PoemTown.API.CustomMiddleware;

public class ValidateJwtTokenMiddleware
{
    private readonly RequestDelegate _next;
    public ValidateJwtTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        IUnitOfWork _unitOfWork = context.RequestServices.GetRequiredService<IUnitOfWork>();
        ITokenService tokenService = context.RequestServices.GetRequiredService<ITokenService>();
        string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (!string.IsNullOrWhiteSpace(token))
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            if (!jwtHandler.CanReadToken(token))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
        
            var jwtToken = jwtHandler.ReadJwtToken(token);
            var payload = jwtToken.Claims.ToList();
        
            string? userId = StringHelper.CapitalizeString(payload.FirstOrDefault(p => p.Type == "UserId")?.Value);
            string? tokenHash = payload.FirstOrDefault(p => p.Type == "TokenHash")?.Value;
            string userAgentHash = HmacSHA256Hasher.Hash(StringHelper.NormalizeString(context.Request.Headers["User-Agent"].ToString()));

            string ipAddress = (context.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim()
                                ?? context.Connection.RemoteIpAddress?.ToString())!;
            
            string ipAddressHash = HmacSHA256Hasher.Hash(ipAddress);
            
            var userToken = await _unitOfWork.GetRepository<UserToken>()
                .FindAsync(p =>
                    p.UserId.ToString() == userId && p.UserAgentHash == userAgentHash &&
                    p.IpAddressHash == ipAddressHash);
            
            if(userToken == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
            
            if (String.CompareOrdinal(userToken.ExpirationTime, TimeStampHelper.GenerateUnixTimeStampNow().ToString()) < 0)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            // Validate the token hash with the input string
            // InputString is a string is the combination of toUpper(userId) + UserAgentHash + IpAddressHash + value(refreshToken)

            string inputString = StringHelper.CapitalizeString(userId) + userAgentHash + ipAddressHash + userToken.Value;
            
            if (!tokenService.ValidTokenHash(tokenHash ?? "", inputString))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
        }
        await _next(context); // Call the next middleware

    }
}
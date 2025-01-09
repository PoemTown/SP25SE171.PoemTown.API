using Microsoft.AspNetCore.Identity;
using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;

namespace PoemTown.Repository.Entities;

public class UserToken : IdentityUserToken<Guid>, IBaseEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? ExpirationTime { get; set; }
    public string? IpAddressHash { get; set; }
    public string? UserAgentHash { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastUpdatedBy { get; set; }
    public string? DeletedBy { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset LastUpdatedTime { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
    
    public UserToken()
    {
        CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
    }
}
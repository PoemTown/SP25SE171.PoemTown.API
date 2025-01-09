using Microsoft.AspNetCore.Identity;
using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;

namespace PoemTown.Repository.Entities;

public class UserClaim : IdentityUserClaim<Guid>, IBaseEntity
{
    public string? CreatedBy { get; set; }
    public string? LastUpdatedBy { get; set; }
    public string? DeletedBy { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset LastUpdatedTime { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
    
    public UserClaim()
    {
        CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
    }
}
using Microsoft.AspNetCore.Identity;
using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;

namespace PoemTown.Repository.Entities;

public class User : IdentityUser<Guid>, IBaseEntity
{
    public string? FullName { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Salt { get; set; }
    public string? GoogleId { get; set; }
    
    public string? EmailOtp { get; set; }
    public string? EmailOtpExpiration { get; set; }
    
    public string? PhoneOtp { get; set; }
    public string? PhoneOtpExpiration { get; set; }
    
    public string? CreatedBy { get; set; }
    public string? LastUpdatedBy { get; set; }
    public string? DeletedBy { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset LastUpdatedTime { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }

    public User()
    {
        CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
    }
}
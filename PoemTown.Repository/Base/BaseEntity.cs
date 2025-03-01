using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System.ComponentModel.DataAnnotations;

namespace PoemTown.Repository.Base;

public class BaseEntity : IBaseEntity
{
    public string? CreatedBy { get; set; }
    public string? LastUpdatedBy { get; set; }
    public string? DeletedBy { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset LastUpdatedTime { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }

    public BaseEntity()
    {
        CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
    }
}
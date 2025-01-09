namespace PoemTown.Repository.Base.Interfaces;

public interface IBaseEntity
{
    string? CreatedBy { get; set; }
    string? LastUpdatedBy { get; set; }
    string? DeletedBy { get; set; }
    DateTimeOffset CreatedTime { get; set; }
    DateTimeOffset LastUpdatedTime { get; set; }
    DateTimeOffset? DeletedTime { get; set; }
}
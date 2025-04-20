using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class DailyMessage : BaseEntity
{
    public Guid Id { get; set; }
    public string? Message { get; set; } = String.Empty;
    public bool? IsInUse { get; set; } = false;
}
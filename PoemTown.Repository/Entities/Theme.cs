using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class Theme : BaseEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public bool IsInUse { get; set; } = false;
    public bool? IsDefault { get; set; } = false;
    
    public Guid UserId { get; set; }
    
    public virtual User User { get; set; } = null!;
    public virtual ICollection<ThemeUserTemplateDetail>? ThemeUserTemplateDetails { get; set; }
}
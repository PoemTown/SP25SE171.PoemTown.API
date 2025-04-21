using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class PoemType : BaseEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public string? GuideLine { get; set; }
    
    public virtual ICollection<Poem>? Poems { get; set; }
}
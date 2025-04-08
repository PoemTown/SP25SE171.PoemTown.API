using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class PoetSample : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Bio { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string? Avatar { get; set; }
    
    public virtual ICollection<Collection>? Collections { get; set; }
    public virtual ICollection<Poem>? Poems { get; set; }
}
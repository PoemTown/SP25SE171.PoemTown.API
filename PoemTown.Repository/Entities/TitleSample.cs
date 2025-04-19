using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class TitleSample : BaseEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = "";
    
    public virtual ICollection<PoetSampleTitleSample>? PoetSampleTitleSamples { get; set; }
}
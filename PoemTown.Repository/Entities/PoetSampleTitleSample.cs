using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class PoetSampleTitleSample : BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid PoetSampleId { get; set; }
    public Guid TitleSampleId { get; set; }
    
    public virtual PoetSample? PoetSample { get; set; }
    public virtual TitleSample? TitleSample { get; set; }
}
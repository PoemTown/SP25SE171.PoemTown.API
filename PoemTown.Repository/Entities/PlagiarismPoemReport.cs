using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class PlagiarismPoemReport : BaseEntity
{
    public Guid Id { get; set; }
    public double Score { get; set; }
    public Guid ReportId { get; set; }
    public virtual Report Report { get; set; } = null!;
    
    public Guid PlagiarismFromPoemId { get; set; }
    public virtual Poem PlagiarismFromPoem { get; set; } = null!;
}
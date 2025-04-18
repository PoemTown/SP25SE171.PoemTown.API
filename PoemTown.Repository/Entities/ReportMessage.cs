using PoemTown.Repository.Base;
using PoemTown.Repository.Enums.Reports;

namespace PoemTown.Repository.Entities;

public class ReportMessage : BaseEntity
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public ReportType? Type { get; set; }
    
    public virtual ICollection<Report>? Reports { get; set; }
}
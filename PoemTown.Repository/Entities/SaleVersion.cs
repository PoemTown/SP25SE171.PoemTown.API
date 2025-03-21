using System.Collections;
using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class SaleVersion : BaseEntity
{
    public Guid Id { get; set; }
    public int CommissionPercentage { get; set; }
    public decimal Price { get; set; }
    public int DurationTime { get; set; }
    
    public Guid PoemId { get; set; }
    
    public virtual Poem Poem { get; set; } = null!;
    public virtual OrderDetail OrderDetail { get; set; } = null!;
    public virtual ICollection<RecordFile>? RecordFiles { get; set; }
    public virtual ICollection<UsageRight>? UsageRights { get; set; }
}
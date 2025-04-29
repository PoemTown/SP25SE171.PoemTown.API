using PoemTown.Repository.Base;
using PoemTown.Repository.Enums.WithdrawalComplaints;

namespace PoemTown.Repository.Entities;

public class WithdrawalComplaint : BaseEntity
{
    public Guid Id { get; set; }
    public WithdrawalComplaintStatus Status { get; set; }
    public string? Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? ResolveDescription { get; set; } = string.Empty;
    
    public Guid? WithdrawalFormId { get; set; }
    public virtual WithdrawalForm? WithdrawalForm { get; set; }
    
    public virtual ICollection<WithdrawalComplaintImage>? Images { get; set; }
}
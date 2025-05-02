using PoemTown.Repository.Base;
using PoemTown.Repository.Enums.WithdrawalComplaints;

namespace PoemTown.Repository.Entities;

public class WithdrawalComplaintImage : BaseEntity
{
    public Guid Id { get; set; }
    public string? Image { get; set; }
    public WithdrawalComplaintType Type { get; set; }
    
    public Guid? WithdrawalComplaintId { get; set; }
    public virtual WithdrawalComplaint? WithdrawalComplaint { get; set; } = null!;
}
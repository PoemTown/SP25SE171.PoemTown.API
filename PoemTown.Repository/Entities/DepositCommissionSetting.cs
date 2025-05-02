using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class DepositCommissionSetting : BaseEntity
{
    public Guid Id { get; set; }
    public int AmountPercentage { get; set; } = 5 / 100;
    public bool? IsInUse { get; set; } = false;
    
    public virtual ICollection<Transaction>? Transactions { get; set; }
}
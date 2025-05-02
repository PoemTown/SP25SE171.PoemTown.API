using PoemTown.Repository.Base;
using PoemTown.Repository.Enums.WithdrawalForm;

namespace PoemTown.Repository.Entities;

public class WithdrawalForm : BaseEntity
{
    public Guid Id { get; set; }
    public WithdrawalFormStatus Status { get; set; }
    public decimal Amount { get; set; }
    //public BankType BankType { get; set; }
    public string? Description { get; set; }
    public string? ResolveDescription { get; set; }
    public string? ResolveEvidence { get; set; }
    public string AccountName { get; set; } = null!;
    public string AccountNumber { get; set; } = null!;
    
    public Guid UserEWalletId { get; set; }
    public Guid? BankTypeId { get; set; }
    public Guid? UserBankTypeId { get; set; }
    
    public virtual BankType? BankType { get; set; }
    public virtual UserBankType? UserBankType { get; set; }
    public virtual UserEWallet UserEWallet { get; set; } = null!;
    public virtual ICollection<Announcement>? Announcements { get; set; }
    
    public virtual ICollection<WithdrawalComplaint>? WithdrawalComplaints { get; set; }
}
using PoemTown.Repository.Base;
using PoemTown.Repository.Enums.BankTypes;

namespace PoemTown.Repository.Entities;

public class BankType : BaseEntity
{
    public Guid Id { get; set; }
    public string? BankName { get; set; } = String.Empty;
    public string? BankCode { get; set; } = String.Empty;
    public string? ImageIcon { get; set; } = String.Empty;
    public BankTypeStatus Status { get; set; } = BankTypeStatus.Inactive;
    
    public virtual ICollection<UserBankType>? UserBankTypes { get; set; }
    public virtual ICollection<WithdrawalForm>? WithdrawalForms { get; set; }
}
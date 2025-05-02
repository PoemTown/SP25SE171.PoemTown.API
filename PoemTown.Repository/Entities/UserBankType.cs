using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class UserBankType : BaseEntity
{
    public Guid Id { get; set; }
    public string AccountNumber { get; set; } = String.Empty;
    public string AccountName { get; set; } = String.Empty;
    
    public Guid? BankTypeId { get; set; }
    public Guid? UserId { get; set; }
    
    public virtual BankType? BankType { get; set; }
    public virtual User? User { get; set; }
    
    public virtual ICollection<WithdrawalForm>? WithdrawalForms { get; set; }
}
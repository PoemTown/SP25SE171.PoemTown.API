using PoemTown.Repository.Enums.WithdrawalForm;

namespace PoemTown.Service.BusinessModels.RequestModels.WithdrawalFormRequests;

public class CreateWithdrawalFormRequest
{
    public decimal Amount { get; set; }
    public Guid BankTypeId { get; set; }
    public Guid? UserBankTypeId { get; set; }
    public string? Description { get; set; }
    public string? AccountName { get; set; }
    public string? AccountNumber { get; set; }
}
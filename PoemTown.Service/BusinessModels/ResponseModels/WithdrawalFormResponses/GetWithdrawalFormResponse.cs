using PoemTown.Repository.Enums.WithdrawalForm;

namespace PoemTown.Service.BusinessModels.ResponseModels.WithdrawalFormResponses;

public class GetWithdrawalFormResponse
{
    public Guid Id { get; set; }
    public WithdrawalFormStatus Status { get; set; }
    public decimal Amount { get; set; }
    public BankType BankType { get; set; }
    public string? Description { get; set; }
    public string? ResolveDescription { get; set; }
    public string? ResolveEvidence { get; set; }
    public string AccountName { get; set; } = null!;
    public string AccountNumber { get; set; } = null!;
    public DateTimeOffset CreatedTime { get; set; }
}
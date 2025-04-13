using PoemTown.Repository.Enums.WithdrawalForm;

namespace PoemTown.Service.BusinessModels.RequestModels.WithdrawalFormRequests;

public class ResolveWithdrawalFormRequest
{
    public Guid Id { get; set; }
    public WithdrawalFormStatus Status { get; set; }
    public string? ResolveDescription { get; set; }
    public string? ResolveEvidence { get; set; }
}
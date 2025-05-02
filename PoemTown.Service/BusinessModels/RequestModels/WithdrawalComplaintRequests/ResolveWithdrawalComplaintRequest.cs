using PoemTown.Repository.Enums.WithdrawalComplaints;

namespace PoemTown.Service.BusinessModels.RequestModels.WithdrawalComplaintRequests;

public class ResolveWithdrawalComplaintRequest
{
    public string ResolveDescription { get; set; } = string.Empty;
    public WithdrawalComplaintStatus Status { get; set; }
    public IList<string>? Images { get; set; }
}
using PoemTown.Repository.Enums.WithdrawalComplaints;

namespace PoemTown.Service.BusinessModels.ResponseModels.WithdrawalComplaintResponses;

public class GetWithdrawalComplaintImageResponse
{
    public Guid Id { get; set; }
    public string? Image { get; set; }
    public WithdrawalComplaintType Type { get; set; }
}
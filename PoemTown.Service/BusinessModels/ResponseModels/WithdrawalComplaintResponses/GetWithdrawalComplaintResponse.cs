using PoemTown.Repository.Enums.WithdrawalComplaints;

namespace PoemTown.Service.BusinessModels.ResponseModels.WithdrawalComplaintResponses;

public class GetWithdrawalComplaintResponse
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ResolveDescription { get; set; }
    public WithdrawalComplaintStatus? Status { get; set; }
    public DateTimeOffset? CreatedTime { get; set; }
    public IList<GetWithdrawalComplaintImageResponse>? ComplaintImages { get; set; }
    public IList<GetWithdrawalComplaintImageResponse>? ResolveImages { get; set; }
}
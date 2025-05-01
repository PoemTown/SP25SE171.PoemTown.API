namespace PoemTown.Service.BusinessModels.RequestModels.WithdrawalComplaintRequests;

public class UpdateWithdrawalComplaintRequest
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
}
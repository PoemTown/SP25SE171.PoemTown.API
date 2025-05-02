namespace PoemTown.Service.BusinessModels.RequestModels.WithdrawalComplaintRequests;

public class CreateNewWithdrawalComplaintRequest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public IList<string>? Images { get; set; }
}
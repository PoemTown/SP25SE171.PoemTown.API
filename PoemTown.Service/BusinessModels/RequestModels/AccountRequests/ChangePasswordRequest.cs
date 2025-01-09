namespace PoemTown.Service.BusinessModels.RequestModels.AccountRequests;

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }

}
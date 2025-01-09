using System.ComponentModel.DataAnnotations;

namespace PoemTown.Service.BusinessModels.RequestModels.AccountRequests;

public class NewPasswordForgotRequest
{
    [EmailAddress]
    public string Email { get; set; }
    public string NewPassword { get; set; }
    public string ResetPasswordToken { get; set; }
    public string ExpiredTimeStamp { get; set; }
}
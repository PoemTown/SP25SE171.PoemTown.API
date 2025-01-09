namespace PoemTown.Service.BusinessModels.ViewTemplateModels;

public class EmailForgotPassword
{
    public string Email { get; set; }
    public string ResetPasswordToken { get; set; }
    public string ExpiredTimeStamp { get; set; }
}
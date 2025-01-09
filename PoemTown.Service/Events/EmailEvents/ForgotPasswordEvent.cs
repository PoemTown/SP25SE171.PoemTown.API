namespace PoemTown.Service.Events.EmailEvents;

public class ForgotPasswordEvent
{
    public string Email { get; set; }
    public string ResetPasswordToken { get; set; }
    public string ExpiredTimeStamp { get; set; }
}
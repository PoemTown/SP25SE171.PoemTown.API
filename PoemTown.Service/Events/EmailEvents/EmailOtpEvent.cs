namespace PoemTown.Service.Events.EmailEvents;

public class EmailOtpEvent
{
    public string Email { get; set; }
    public string EmailOtp { get; set; }
}
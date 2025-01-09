using MassTransit;
using PoemTown.Service.BusinessModels.ViewTemplateModels;
using PoemTown.Service.Events.EmailEvents;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Consumers.EmailConsumers;

public class EmailOtpConsumer : IConsumer<EmailOtpEvent>
{
    private readonly IEmailService _emailService;
    public EmailOtpConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }
    
    public async Task Consume(ConsumeContext<EmailOtpEvent> context)
    {
        var message = context.Message;
        var emailOtp = new EmailOtp()
        {
            Email = message.Email,
            Otp = message.EmailOtp
        };

        await _emailService.SendOtp(emailOtp);
    }
}
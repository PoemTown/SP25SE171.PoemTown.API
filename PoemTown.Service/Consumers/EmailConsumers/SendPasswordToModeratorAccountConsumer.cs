using MassTransit;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.ViewTemplateModels;
using PoemTown.Service.Events.EmailEvents;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Consumers.EmailConsumers;

public class SendPasswordToModeratorAccountConsumer : IConsumer<SendPasswordToModeratorAccountEvent>
{
    private readonly IEmailService _emailService;
    public SendPasswordToModeratorAccountConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }
    
    public async Task Consume(ConsumeContext<SendPasswordToModeratorAccountEvent> context)
    {
        var message = context.Message;
        
        SendPasswordToModeratorAccount model = new SendPasswordToModeratorAccount()
        {
            FullName = message.FullName,
            Email = message.Email,
            Password = message.Password
        };
        
        await _emailService.SendEmailModeratorAccountPassword(model);
    }
}
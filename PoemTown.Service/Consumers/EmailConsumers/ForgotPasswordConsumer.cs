using MassTransit;
using PoemTown.Service.Events.EmailEvents;
using PoemTown.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Service.BusinessModels.ViewTemplateModels;

namespace PoemTown.Service.Consumers.EmailConsumers
{
    public class ForgotPasswordConsumer : IConsumer<ForgotPasswordEvent>
    {
        private readonly IEmailService _emailService;
        public ForgotPasswordConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public async Task Consume(ConsumeContext<ForgotPasswordEvent> context)
        {
            var message = context.Message;
            var emailForgotPassword = new EmailForgotPassword()
            {
                Email = message.Email,
                ResetPasswordToken = message.ResetPasswordToken,
                ExpiredTimeStamp = message.ExpiredTimeStamp
            };
            await _emailService.SendEmailForgotPassword(emailForgotPassword);
        }
    }
}

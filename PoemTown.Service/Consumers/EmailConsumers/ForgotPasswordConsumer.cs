using MassTransit;
using PoemTown.Service.Events.EmailEvents;
using PoemTown.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PoemTown.Service.BusinessModels.ViewTemplateModels;

namespace PoemTown.Service.Consumers.EmailConsumers
{
    public class ForgotPasswordConsumer : IConsumer<ForgotPasswordEvent>
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ForgotPasswordConsumer(IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
        }
        public async Task Consume(ConsumeContext<ForgotPasswordEvent> context)
        {
            var message = context.Message;
            var emailForgotPassword = new EmailForgotPassword()
            {
                FrontendUrl = _configuration.GetSection("FrontEndDomain:Host").Value ?? "",
                Email = message.Email,
                ResetPasswordToken = message.ResetPasswordToken,
                ExpiredTimeStamp = message.ExpiredTimeStamp
            };
            await _emailService.SendEmailForgotPassword(emailForgotPassword);
        }
    }
}

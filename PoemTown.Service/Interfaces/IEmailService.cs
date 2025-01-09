using PoemTown.Service.BusinessModels.ViewTemplateModels;

namespace PoemTown.Service.Interfaces;

public interface IEmailService
{
    Task SendOtp(EmailOtp emailOtp);
    Task SendEmailForgotPassword(EmailForgotPassword emailForgotPassword);
}
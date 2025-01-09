using System.Net.Mail;
using PoemTown.Service.BusinessModels.ConfigurationModels.Email;
using PoemTown.Service.BusinessModels.ViewTemplateModels;
using PoemTown.Service.Interfaces;
using RazorLight;

namespace PoemTown.Service.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly RazorLightEngine _razorLightEngine;
    private readonly SmtpClient _smtpClient;
    public EmailService(EmailSettings emailSettings, RazorLightEngine razorLightEngine, SmtpClient smtpClient)
    {
        _emailSettings = emailSettings;
        _razorLightEngine = razorLightEngine;
        _smtpClient = smtpClient;
    }
    
    public async Task<string> RenderEmailTemplateAsync<T>(string templateName, T model)
    {
        // Render the Razor view with the provided model
        string templatePath = $"{templateName}.cshtml";
        return await _razorLightEngine.CompileRenderAsync(templatePath, model);
    }
    
    public Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.Username),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(toEmail);

        return _smtpClient.SendMailAsync(mailMessage);
    }

    public async Task SendOtp(EmailOtp emailOtp)
    {
        string body = await RenderEmailTemplateAsync("EmailOtpTemplate", emailOtp);
        await SendEmailAsync(emailOtp.Email, "Your 2-Step verification code", body);
    }

    public async Task SendEmailForgotPassword(EmailForgotPassword emailForgotPassword)
    {
        string body = await RenderEmailTemplateAsync("EmailForgotPasswordTemplate", emailForgotPassword);
        await SendEmailAsync(emailForgotPassword.Email, "Reset your password", body);
    }
}
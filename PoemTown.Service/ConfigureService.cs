using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoemTown.Service.BusinessModels.ConfigurationModels.Email;
using PoemTown.Service.BusinessModels.ConfigurationModels.RabbitMQ;
using PoemTown.Service.BusinessModels.MappingModels;
using PoemTown.Service.Consumers.EmailConsumers;
using PoemTown.Service.Interfaces;
using PoemTown.Service.Services;
using RazorLight;

namespace PoemTown.Service;

public static class ConfigureService
{
    public static void AddConfigureServiceService(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddAutoMapperConfig(configuration);
        services.AddRabbitMqSettings(configuration);
        services.AddMasstransitRabbitMq(configuration);
        services.AddDependencyInjection();
        services.AddEmailSettingsConfig(configuration);
        services.AddRazorLightEngine(env);
        services.AddSmtpClient();
    }
    
    private static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IAuthenService, AuthenService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IPoemService, PoemService>();
    }
    
    private static void AddAutoMapperConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(PaginationMapping));
    }
    
    private static void AddMasstransitRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(config =>
        {
            //Register consumers
            config.AddConsumer<EmailOtpConsumer>();
            config.AddConsumer<ForgotPasswordConsumer>();
            //config rabbitmq host
            config.UsingRabbitMq((context, cfg) =>
            {
                var rbmqSettings = context.GetRequiredService<RabbitMQSettings>();
                cfg.Host(rbmqSettings.Host, ushort.Parse(rbmqSettings.Port),
                    rbmqSettings.VirtualHost, h =>
                    {
                        h.Username(rbmqSettings.Username);
                        h.Password(rbmqSettings.Password);
                    });
                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddMassTransitHostedService();
    }
    private static void AddRabbitMqSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var rbmqConfig = configuration.GetSection(RabbitMQSettings.ConfigSection);
        services.AddSingleton<RabbitMQSettings>(options =>
        {
            var rbmqSettings = new RabbitMQSettings()
            {
                Port = rbmqConfig.GetSection("Port").Value,
                Host = rbmqConfig.GetSection("Host").Value,
                Username = rbmqConfig.GetSection("Username").Value,
                Password = rbmqConfig.GetSection("Password").Value,
                VirtualHost = rbmqConfig.GetSection("VirtualHost").Value
            };
            return rbmqSettings;
        });
    }
    
    private static void AddEmailSettingsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<EmailSettings>(options =>
        {
            return new EmailSettings
            {
                Username = configuration["Email:Username"]!,
                Password = configuration["Email:Password"]!,
                SenderName = configuration["Email:SenderName"]!,
                Host = configuration["Email:Host"]!,
                Port = int.Parse(configuration["Email:Port"]!)
            };
        });
    }
    
    private static void AddRazorLightEngine(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddSingleton<RazorLightEngine>(options =>
        {
            string root = env.ContentRootPath;
            Console.WriteLine(root);
            return new RazorLightEngineBuilder()
                .UseFileSystemProject(Path.Combine(env.ContentRootPath, "Views"))
                .UseMemoryCachingProvider()
                .Build();
        });
    }
    
    private static void AddSmtpClient(this IServiceCollection services)
    {
        services.AddSingleton<SmtpClient>(options =>
        {
            var emailSettings = options.GetRequiredService<EmailSettings>();
            var smtpClient = new SmtpClient();
            smtpClient.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtpClient.Authenticate(emailSettings.Username, emailSettings.Password);

            return smtpClient;
        });
    }
}
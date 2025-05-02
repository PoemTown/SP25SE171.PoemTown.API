using System.Net;
using Amazon.S3;
using Betalgo.Ranul.OpenAI.Extensions;
using MailKit.Net.Smtp;
using MailKit.Security;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ConfigurationModels.Email;
using PoemTown.Service.BusinessModels.ConfigurationModels.Payment;
using PoemTown.Service.BusinessModels.ConfigurationModels.RabbitMQ;
using PoemTown.Service.BusinessModels.MappingModels;
using PoemTown.Service.BusinessModels.ViewTemplateModels;
using PoemTown.Service.Consumers.AnnouncementConsumers;
using PoemTown.Service.Consumers.AuthenticationConsumers;
using PoemTown.Service.Consumers.CollectionConsumers;
using PoemTown.Service.Consumers.EmailConsumers;
using PoemTown.Service.Consumers.OrderConsumers;
using PoemTown.Service.Consumers.PoemConsumers;
using PoemTown.Service.Consumers.TemplateConsumers;
using PoemTown.Service.Consumers.ThemeConsumers;
using PoemTown.Service.Consumers.TransactionConsumers;
using PoemTown.Service.Consumers.UserEWalletConsumers;
using PoemTown.Service.Events.ThemeEvents;
using PoemTown.Service.Interfaces;
using PoemTown.Service.Scheduler.AchievementJobs;
using PoemTown.Service.Scheduler.LeaderBoardJobs;
using PoemTown.Service.PlagiarismDetector.Interfaces;
using PoemTown.Service.PlagiarismDetector.Services;
using PoemTown.Service.PlagiarismDetector.Settings;
using PoemTown.Service.Scheduler.DailyMessageJobs;
using PoemTown.Service.Scheduler.PaymentJobs;
using PoemTown.Service.Services;
using PoemTown.Service.ThirdParties.Interfaces;
using PoemTown.Service.ThirdParties.Services;
using PoemTown.Service.ThirdParties.Settings.AwsS3;
using PoemTown.Service.ThirdParties.Settings.TheHiveAi;
using PoemTown.Service.ThirdParties.Settings.VnPay;
using PoemTown.Service.ThirdParties.Settings.ZaloPay;
using Qdrant.Client;
using Quartz;
using RazorLight;
using PoemTown.Service.Scheduler.UsageRightJobs;
using PoemTown.Service.ThirdParties.Settings.Stripe;
using Stripe;
using AccountService = PoemTown.Service.Services.AccountService;
using TokenService = PoemTown.Service.Services.TokenService;

namespace PoemTown.Service;

public static class ConfigureService
{
    public static void AddConfigureServiceService(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment env)
    {
        services.AddAutoMapperConfig(configuration);
        services.AddRabbitMqSettings(configuration);
        services.AddMasstransitRabbitMq(configuration);
        services.AddDependencyInjection();
        services.AddEmailSettingsConfig(configuration);
        services.AddRazorLightEngine(env);
        services.AddSmtpClient();
        services.AddAwsS3Configuration(configuration);
        services.AddZaloPayConfig(configuration);
        services.AddPaymentRedirectConfig(configuration);
        services.AddQuartzConfig();

        services.AddBetalgoOpenAI(configuration);
        services.AddTheHiveAiSettings(configuration);
        services.AddSignalRConfig();
        services.AddQDrantConfig(configuration);
        services.AddVnPayConfig(configuration);
        services.AddStripeConfig(configuration);
    }

    private static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IAuthenService, AuthenService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IPoemService, PoemService>();
        services.AddScoped<ICollectionService, CollectionService>();
        services.AddScoped<IPoemHistoryService, PoemHistoryService>();
        services.AddScoped<ILikeService, LikeService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ITargetMarkService, TargetMarkService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IFollowerService, FollowerService>();
        services.AddScoped<IStatisticService, StatisticService>();
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IThemeService, ThemeService>();
        services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();
        services.AddScoped<IUserEWalletService, UserEWalletService>();
        services.AddScoped<PaymentMethodFactory>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IRecordFileService, RecordFileService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<ILeaderBoardService, LeaderBoardService>();
        services.AddScoped<IAchievementService, AchievementService>();
        services.AddScoped<IUsageRightService, UsageRightService>();
        services.AddScoped<IAnnouncementService, AnnouncementService>();
        services.AddScoped<IPoetSampleService, PoetSampleService>();
        services.AddScoped<IWithdrawalFormService, WithdrawalFormService>();
        services.AddScoped<IPoemTypeService, PoemTypeService>();
        services.AddScoped<ITitleSampleService, TitleSampleService>();
        services.AddScoped<ISystemContactService, SystemContactService>();
        services.AddScoped<IDailyMessageService, DailyMessageService>();
        services.AddScoped<IContentPageService, ContentPageService>();
        services.AddScoped<IDepositCommissionSettingService, DepositCommissionSettingService>();
        services.AddScoped<IWithdrawalComplaintService, WithdrawalComplaintService>();
        services.AddScoped<IBankTypeService, BankTypeService>();

        //Plagiarism detector
        services.AddScoped<IEmbeddingService, EmbeddingService>();
        services.AddScoped<IQDrantService, QDrantService>();

        //Third parties
        services.AddScoped<IAwsS3Service, AwsS3Service>();
        services.AddScoped<IZaloPayService, ZaloPayService>();
        services.AddScoped<IVnPayService, VnPayService>();
        services.AddScoped<VnPayService>();
        services.AddScoped<ZaloPayService>();
        services.AddScoped<IStripeService, StripeService>();
        services.AddScoped<StripeService>();
        services.AddScoped<ITheHiveAiService, TheHiveAiService>();
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
            config.AddConsumer<CreateDefaultCollectionConsumer>();
            config.AddConsumer<CreateDefaultUserThemeConsumer>();
            config.AddConsumer<InitialUserEWalletConsumer>();
            config.AddConsumer<AddUserTemplateDetailConsumer>();
            config.AddConsumer<UpdatePaidOrderAndCreateTransactionConsumer>();
            config.AddConsumer<UpdateCancelledOrderConsumer>();
            config.AddConsumer<CreateDonateTransactionConsumer>();
            config.AddConsumer<CreateOrderConsumer>();
            config.AddConsumer<CreateTransactionConsumer>();
            config.AddConsumer<SendPasswordToModeratorAccountConsumer>();
            config.AddConsumer<CheckPoemPlagiarismConsumer>();
            config.AddConsumer<TrackingUserLoginConsumer>();
            config.AddConsumer<CreateCommissionConsumer>();
            config.AddConsumer<SendUserAnnouncementConsumer>();
            config.AddConsumer<SendBulkUserAnnouncementConsumer>();
            config.AddConsumer<UpdateAndSendUserAnnouncementConsumer>();
            config.AddConsumer<UpdatePaidTransactionConsumer>();
            config.AddConsumer<UpdateCancelledTransactionConsumer>();
            config.AddConsumer<StorePoemIntoQDrantConsumer>();
            config.AddConsumer<DeletePoemPointInQDrantConsumer>();

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
        services.AddScoped<SmtpClient>(options =>
        {
            var emailSettings = options.GetRequiredService<EmailSettings>();
            var smtpClient = new SmtpClient();
            smtpClient.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtpClient.Authenticate(emailSettings.Username, emailSettings.Password);

            return smtpClient;
        });
    }

    private static void AddAwsS3Configuration(this IServiceCollection services, IConfiguration configuration)
    {
        var s3Config = configuration.GetSection("AwsS3Settings");
        services.AddSingleton<AwsS3Settings>(options =>
        { 
            var awsS3Setting = new AwsS3Settings()
            {
                AccessKey = s3Config.GetSection("AccessKey").Value,
                BucketName = s3Config.GetSection("BucketName").Value,
                SecretKey = s3Config.GetSection("SecretKey").Value,
                ServiceUrl = s3Config.GetSection("ServiceUrl").Value
            };
            awsS3Setting.IsValid();
            return awsS3Setting;
        });

        services.AddSingleton<IAmazonS3>(options =>
        {
            var config = options.GetRequiredService<AwsS3Settings>();
            var amazonConfig = new AmazonS3Config
            {
                ServiceURL = config.ServiceUrl,
                ForcePathStyle = true,
            };
            return new AmazonS3Client(config.AccessKey, config.SecretKey, amazonConfig);
        });
    }

    private static void AddZaloPayConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var zaloPayConfig = configuration.GetSection("ZaloPay");
        services.AddSingleton<ZaloPaySettings>(options =>
        {
            var zaloPaySettings = new ZaloPaySettings
            {
                AppId = zaloPayConfig.GetSection("AppId").Value,
                Key1 = zaloPayConfig.GetSection("Key1").Value,
                Key2 = zaloPayConfig.GetSection("Key2").Value,
                CallbackUrl = zaloPayConfig.GetSection("CallbackUrl").Value,
                ZalopayCreateOrderUrl = zaloPayConfig.GetSection("ZalopayCreateOrderUrl").Value
            };
            zaloPaySettings.IsValid();
            return zaloPaySettings;
        });
    }

    private static void AddPaymentRedirectConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var paymentRedirectConfig = configuration.GetSection("PaymentRedirect");
        services.AddSingleton<PaymentRedirectSettings>(options =>
        {
            var paymentRedirectSettings = new PaymentRedirectSettings
            {
                RedirectSuccessUrl = paymentRedirectConfig.GetSection("RedirectSuccessUrl").Value,
                RedirectFailureUrl = paymentRedirectConfig.GetSection("RedirectFailureUrl").Value
            };
            return paymentRedirectSettings;
        });
    }

    private static void AddQuartzConfig(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            // Define job keys.
            var leaderBoardJobKey = new JobKey("LeaderBoardCalculationJob", "LeaderBoard");
            var achievementJobKey = new JobKey("MonthlyAchievementJob", "Achievement");
            var usageRightJobKey = new JobKey("TimeOutUsageRightJob", "TimeOut");
            var updateInUseDailyMessageJobKey = new JobKey("UpdateInUseDailyMessageJob", "DailyMessage");

            // Register the jobs.
            q.AddJob<MonthlyAchievementJob>(opts => opts.WithIdentity(achievementJobKey));
            q.AddJob<LeaderBoardCalculationJob>(opts => opts.WithIdentity(leaderBoardJobKey));
            q.AddJob<TimeOutUsageRightJob>(opts => opts.WithIdentity(usageRightJobKey));
            q.AddJob<UpdateInUseDailyMessageJob>(opts => opts.WithIdentity(updateInUseDailyMessageJobKey));

            
            // Trigger for LeaderBoardCalculationJob: fire immediately and every 30 seconds.
            q.AddTrigger(opts => opts
                .ForJob(leaderBoardJobKey)
                .WithIdentity("LeaderBoardCalculationJobTrigger", "LeaderBoard")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(30)
                    .RepeatForever()));

            // Trigger for MonthlyAchievementJob: for testing, fire every 1 minute.
            // In AddQuartzConfig()
            q.AddTrigger(opts => opts
                .ForJob(achievementJobKey)
                .WithIdentity("MonthlyAchievementJobTrigger", "Achievement")
                .WithCronSchedule("0 0 0 1 * ?", cron =>
                    cron.InTimeZone(
                        TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")))); // Use your time zone

            q.AddTrigger(opts => opts
                    .ForJob(usageRightJobKey)
                    .WithIdentity("TimeOutUsageRightJob", "TimeOut")
                    .WithCronSchedule("0 0/5 * * * ?", cron =>
                     cron.InTimeZone(
                         TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"))));
            
            q.AddTrigger(opts => opts
                .ForJob(updateInUseDailyMessageJobKey)
                .WithIdentity("UpdateInUseDailyMessageTrigger", "DailyMessage")
                .WithCronSchedule("0 0 0 * * ?", x =>
                {
                    x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
                }));

        });
        services.AddQuartzHostedService(p => p.WaitForJobsToComplete = true);

        services.AddScoped<PaymentTimeOutJob>();
        services.AddScoped<LeaderBoardCalculationJob>();
        services.AddScoped<MonthlyAchievementJob>();
        services.AddScoped<TimeOutUsageRightJob>();
        services.AddScoped<UpdateInUseDailyMessageJob>();
    }

    private static void AddSignalRConfig(this IServiceCollection services)
    {
        services.AddSignalR();
    }

    private static void AddBetalgoOpenAI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenAIService(options =>
        {
            options.ApiKey = configuration.GetSection("OpenAIService:ApiKey").Value
                             ?? throw new ArgumentNullException();
        });
    }

    private static void AddTheHiveAiSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var theHiveAiConfig = configuration.GetSection("TheHiveAiService");
        services.AddSingleton<TheHiveAiSettings>(options =>
        {
            var theHiveAiSettings = new TheHiveAiSettings
            {
                ApiKey = theHiveAiConfig.GetSection("ApiKey").Value ?? "",
                SdxlEnhancedUrl = theHiveAiConfig.GetSection("SdxlEnhancedUrl").Value ?? "",
                FluxSchnellEnhancedUrl = theHiveAiConfig.GetSection("FluxSchnellEnhancedUrl").Value ?? ""
            };
            return theHiveAiSettings;
        });
    }


    private static void AddQDrantConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var qdrantConfig = configuration.GetSection("QDrant");
        services.AddSingleton<QDrantSettings>(options =>
        {
            var qdrantSettings = new QDrantSettings
            {
                Host = qdrantConfig.GetSection("Host").Value,
                Port = int.Parse(qdrantConfig.GetSection("Port").Value),
                ApiKey = qdrantConfig.GetSection("ApiKey").Value
            };
            return qdrantSettings;
        });

        services.AddSingleton<QdrantClient>(options =>
        {
            var qdrantSettings = options.GetRequiredService<QDrantSettings>();
            return new QdrantClient(host: qdrantSettings.Host, port: qdrantSettings.Port,
                apiKey: qdrantSettings.ApiKey);
        });
    }

    private static void AddVnPayConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var vnPayConfig = configuration.GetSection("VnPay");
        services.AddSingleton<VnPaySettings>(options =>
        {
            var vnPaySettings = new VnPaySettings
            {
                Vnp_Url = vnPayConfig.GetSection("Vnp_Url").Value ?? "",
                Vnp_TmnCode = vnPayConfig.GetSection("Vnp_TmnCode").Value ?? "",
                Vnp_HashSecret = vnPayConfig.GetSection("Vnp_HashSecret").Value ?? "",
                Vnp_ReturnUrl = vnPayConfig.GetSection("Vnp_ReturnUrl").Value ?? ""
            };
            return vnPaySettings;
        });
    }
    
    private static void AddStripeConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var stripeConfig = configuration.GetSection("Stripe");
        services.AddSingleton<StripeSettings>(options =>
        {
            var stripeSettings = new StripeSettings
            {
                ApiKey = stripeConfig.GetSection("ApiKey").Value ?? "",
                WebhookSecret = stripeConfig.GetSection("WebhookSecret").Value ?? "",
                WebhookEndpoint = stripeConfig.GetSection("WebhookEndpoint").Value ?? "",
                SuccessUrl = stripeConfig.GetSection("SuccessUrl").Value ?? "",
                CancelUrl = stripeConfig.GetSection("CancelUrl").Value ?? ""
            };
            return stripeSettings;
        });
        services.AddSingleton(new StripeClient(configuration["Stripe:ApiKey"]));
    }
}
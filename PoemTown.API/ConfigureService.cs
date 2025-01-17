using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using PoemTown.API.CustomAttribute;
using PoemTown.API.CustomMiddleware;
using PoemTown.API.CustomModelConvention;
using PoemTown.Repository.Base;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ConfigurationModels.Jwt;

namespace PoemTown.API;

public static class ConfigureService
{
    public static void AddConfigureServiceApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllerConfigs();
        services.AddRouteConfigs();
        services.AddSwaggerConfigs();
        services.AddCorsConfigs();
        services.AddIdentityServerConfig();
        services.AddJwtSettings(configuration);
        services.AddAuthenticationJwt();
        services.AddHttpClientConfig();
        services.AddScoped<ValidateModelStateAttribute>();
    }
    
    public static void AddApplicationApi(this IApplicationBuilder app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                               ForwardedHeaders.XForwardedProto
        });
        
        app.UseCors("AllowAll");
        app.UseHttpsRedirection();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.AddMiddlewareConfigs();

        
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Service API v1");
        });


    }
    
    private static void AddMiddlewareConfigs(this IApplicationBuilder app)
    {
        app.UseMiddleware<ValidateJwtTokenMiddleware>();
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
    private static void AddControllerConfigs(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Conventions.Add(new KebabCaseControllerModelConvention());
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
    }

    private static void AddRouteConfigs(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
        });
    }
    
    private static void AddSwaggerConfigs(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo{ Title = "PoemTown API", Version = "v1" });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your valid token in the text input below.\n\nExample: \"yourTokenHere\"",
            });
            
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }
    
    private static void AddCorsConfigs(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader(); 
                });
        });
    }
    
    private static void AddJwtSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfigs = configuration.GetSection(JwtSettings.ConfigSection);
        services.AddSingleton<JwtSettings>(options =>
        {
            JwtSettings jwtSettings = new JwtSettings
            {
                Key = jwtConfigs.GetSection("Key").Value,
                Issuer = jwtConfigs.GetSection("Issuer").Value,
                Audience = jwtConfigs.GetSection("Audience").Value,
                AccessTokenExpirationMinutes =
                    Convert.ToInt32(jwtConfigs.GetSection("AccessTokenExpiresInMinutes").Value),
                RefreshTokenExpirationHours =
                    Convert.ToInt32(jwtConfigs.GetSection("RefreshTokenExpiresInHours").Value)
            };
            return jwtSettings;
        });
    }
    
    private static void AddAuthenticationJwt(this IServiceCollection services)
    {
        var jwtSettings = services.BuildServiceProvider().GetRequiredService<JwtSettings>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,    
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ClockSkew = TimeSpan.Zero
            };
            
        });
    }
    
    private static void AddHttpClientConfig(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddHttpContextAccessor();
    }

    private static void AddIdentityServerConfig(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options =>
        {
        }).AddEntityFrameworkStores<PoemTownDbContext>()
        .AddSignInManager<SignInManager<User>>()
        .AddDefaultTokenProviders();
    }
}
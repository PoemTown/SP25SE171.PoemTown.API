using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoemTown.Repository.Base;
using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Repositories;

namespace PoemTown.Repository;

public static class ConfigureService
{
    public static void AddConfigureServiceRepository(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddDependencyInjection();
    }
    
    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        if(connectionString == null)
        {
            throw new ArgumentNullException(connectionString);
        }

        services.AddDbContext<PoemTownDbContext>(options =>
        {
            options.UseSqlServer(connectionString).UseLazyLoadingProxies();
        });
    }
    
    public static async Task UseInitializeDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();   
        var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
        await dbInitializer.InitializeAsync();
    }
    
    private static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<DbInitializer>();
    }
}
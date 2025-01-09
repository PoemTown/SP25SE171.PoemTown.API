using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PoemTown.Repository.Utils;

namespace PoemTown.Repository.Base;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PoemTownDbContext>
{
    public DesignTimeDbContextFactory()
    {
        
    }
    public PoemTownDbContext CreateDbContext(string[] args)
    {
        var configuration = ReadConfigurationHelper.ReadDevelopmentAppSettings();
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace("connectionString"))
        {
            connectionString = ReadConfigurationHelper.GetEnvironmentVariable("ConnectionStrings:DefaultConnection");
        }
        //var configuration = CoreHelper.GetDbDesignTimeAppSettings;
        var builder = new DbContextOptionsBuilder<PoemTownDbContext>();
        builder.UseSqlServer(connectionString);
        return new PoemTownDbContext(builder.Options);
    }
}
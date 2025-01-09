using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PoemTown.Repository.Interfaces;

namespace PoemTown.Repository.Base;

public class DbInitializer
{
    private readonly PoemTownDbContext _dbContext;
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DbInitializer(PoemTownDbContext dbContext,
        ILogger<DbInitializer> logger,
        IUnitOfWork unitOfWork
    )
    {
        _dbContext = dbContext;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task InitializeAsync()
    {
        try
        {
            _logger.LogError("Hi from DbInitializer");

            await _dbContext.Database.MigrateAsync();
            //await SeedDataAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in initialize database ", ex);
            throw;
        }
    }
}
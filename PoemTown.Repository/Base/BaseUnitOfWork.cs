using PoemTown.Repository.Base.Interfaces;

namespace PoemTown.Repository.Base;

public class BaseUnitOfWork : IBaseUnitOfWork
{
    private readonly PoemTownDbContext _dbContext;
    private bool _disposed = false;
    public BaseUnitOfWork(PoemTownDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources
                _dbContext.Dispose();
            }

            // Dispose unmanaged resources if any

            _disposed = true;
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
using PoemTown.Repository.Base;
using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Interfaces;

namespace PoemTown.Repository.Repositories;

public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
{
    private readonly PoemTownDbContext _dbContext;

    public UnitOfWork(PoemTownDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public IGenericRepository<T> GetRepository<T>() where T : class, IBaseEntity
    {
        return new GenericRepository<T>(_dbContext);
    }

}
using PoemTown.Repository.Base.Interfaces;

namespace PoemTown.Repository.Interfaces;

public interface IUnitOfWork : IBaseUnitOfWork, IDisposable
{
    IGenericRepository<T> GetRepository<T>() where T : class, IBaseEntity;
}
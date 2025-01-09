namespace PoemTown.Repository.Base.Interfaces;

public interface IBaseUnitOfWork
{
    Task SaveChangesAsync();
    void SaveChanges();
    void Dispose();
}
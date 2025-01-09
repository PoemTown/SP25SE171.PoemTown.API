using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;

namespace PoemTown.Repository.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity
{
    private readonly PoemTownDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(PoemTownDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public T? Find(Expression<Func<T, bool>> predicate, bool isDeleted = false)
    {
        return isDeleted
            ? _dbSet.Where(p => p.DeletedTime != null)
                .FirstOrDefault(predicate)
            : _dbSet.FirstOrDefault(predicate);
    }

    public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate, bool isDeleted = false)
    {
        return isDeleted
            ? await _dbSet.Where(p => p.DeletedTime != null)
                .FirstOrDefaultAsync(predicate)
            : await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public IQueryable<T> AsQueryable()
    {
        return _dbSet.AsQueryable();
    }

    public void Insert(T entity)
    {
        entity.CreatedTime = DateTimeHelper.SystemTimeNow;
        entity.LastUpdatedTime = DateTimeHelper.SystemTimeNow;
        _dbSet.Add(entity);
    }

    public async Task InsertAsync(T entity)
    {
        entity.CreatedTime = DateTimeHelper.SystemTimeNow;
        entity.LastUpdatedTime = DateTimeHelper.SystemTimeNow;
        await _dbSet.AddAsync(entity);
    }
    
    public void Update(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        entity.LastUpdatedTime = DateTimeHelper.SystemTimeNow;
    }
    public void Delete(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        entity.DeletedTime = DateTimeHelper.SystemTimeNow;
    }

    public void DeletePermanent(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Deleted;
    }

    public void InsertRange(IList<T> entities)
    {
        foreach (var entity in entities)
        {
            entity.CreatedTime = DateTimeHelper.SystemTimeNow;
            entity.LastUpdatedTime = DateTimeHelper.SystemTimeNow;
        }
        _dbSet.AddRange(entities);
    }

    public async Task InsertRangeAsync(IList<T> entities)
    {
        foreach (var entity in entities)
        {
            entity.CreatedTime = DateTimeHelper.SystemTimeNow;
            entity.LastUpdatedTime = DateTimeHelper.SystemTimeNow;
        }
        await _dbSet.AddRangeAsync(entities);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            entity.DeletedTime = DateTimeHelper.SystemTimeNow;
        }
    }

    public void DeletePermanentRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }
    }

    public void UpdateDate(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        entity.LastUpdatedTime = DateTimeHelper.SystemTimeNow;
    }
    
    public void SaveChanges() => _dbContext.SaveChanges();
    public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    
    public async Task<PaginationResponse<T>> GetPagination(IQueryable<T> queryable, int pageIndex, int pageSize)
    {
        int totalRecords = await queryable.CountAsync();
        queryable = queryable.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .AsQueryable();

        var data = await queryable.ToListAsync();
        int currentPageRecord = await queryable.CountAsync();
        return new PaginationResponse<T>(data, pageIndex, pageSize, totalRecords, currentPageRecord);
    }
}
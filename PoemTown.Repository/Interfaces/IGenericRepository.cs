using System.Linq.Expressions;
using PoemTown.Repository.Base;

namespace PoemTown.Repository.Interfaces;

public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Finds an entity matching the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <param name="isDeleted">Indicates whether to include logically deleted entities. Default is false.</param>
    /// <returns>The matching entity or null if not found.</returns>
    T? Find(Expression<Func<T, bool>> predicate, bool isDeleted = false);

    /// <summary>
    /// Asynchronously finds an entity matching the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <param name="isDeleted">Indicates whether to include logically deleted entities. Default is false.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the matching entity or null if not found.</returns>
    Task<T?> FindAsync(Expression<Func<T, bool>> predicate, bool isDeleted = false);

    /// <summary>
    /// Returns the queryable collection of entities.
    /// </summary>
    /// <returns>An IQueryable of entities.</returns>
    IQueryable<T> AsQueryable();

    /// <summary>
    /// Inserts a new entity into the data store.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    void Insert(T entity);

    /// <summary>
    /// Asynchronously inserts a new entity into the data store.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task InsertAsync(T entity);

    /// <summary>
    /// Inserts a range of entities into the data store.
    /// </summary>
    /// <param name="entities">The collection of entities to insert.</param>
    void InsertRange(IList<T> entities);

    /// <summary>
    /// Asynchronously inserts a range of entities into the data store.
    /// </summary>
    /// <param name="entities">The collection of entities to insert.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task InsertRangeAsync(IList<T> entities);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(T entity);

    /// <summary>
    /// Updates the last modified date of an entity.
    /// </summary>
    /// <param name="entity">The entity to update the date for.</param>
    void UpdateDate(T entity);

    /// <summary>
    /// Marks an entity as deleted without removing it from the data store.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(T entity);

    /// <summary>
    /// Permanently deletes an entity from the data store.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void DeletePermanent(T entity);

    /// <summary>
    /// Marks a range of entities as deleted without removing them from the data store.
    /// </summary>
    /// <param name="entities">The collection of entities to delete.</param>
    void DeleteRange(IEnumerable<T> entities);

    /// <summary>
    /// Permanently deletes a range of entities from the data store.
    /// </summary>
    /// <param name="entities">The collection of entities to delete.</param>
    void DeletePermanentRange(IEnumerable<T> entities);

    /// <summary>
    /// Saves changes to the data store.
    /// </summary>
    void SaveChanges();

    /// <summary>
    /// Asynchronously saves changes to the data store.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SaveChangesAsync();

    /// <summary>
    /// Retrieves a paginated list of entities.
    /// </summary>
    /// <param name="queryable">The IQueryable to paginate.</param>
    /// <param name="pageIndex">The index of the page to retrieve (1-based).</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paginated response.</returns>
    Task<PaginationResponse<T>> GetPagination(IQueryable<T> queryable, int pageIndex, int pageSize);
}
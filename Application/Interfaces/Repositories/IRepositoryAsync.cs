namespace Application.Interfaces.Repositories;
public interface IRepositoryAsync<T> where T : class
{
    IQueryable<T> Entities { get; }

    Task<List<T>> GetAllAsync(CancellationToken cancellationToken);

    Task<List<T>> GetPagedReponseAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<T> AddAsync(T entity, CancellationToken cancellationToken);

    Task<T[]> AddRangeAsync(CancellationToken cancellationToken, params T[] entities);

    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);

    Task DeleteRangeAsync(params T[] entities);
    Task DeleteRangeAsync(List<T> entities);
}

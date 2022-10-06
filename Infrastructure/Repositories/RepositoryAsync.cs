using Application.Interfaces.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
{
    private readonly FolderContext dbContext;

    public RepositoryAsync(FolderContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IQueryable<T> Entities => dbContext.Set<T>();

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        await dbContext.Set<T>().AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<T[]> AddRangeAsync(CancellationToken cancellationToken, params T[] entities)
    {
        await dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
        return entities;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        await dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
        return entities;
    }

    public Task DeleteAsync(T entity)
    {
        dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(params T[] entities)
    {
        dbContext.Set<T>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(List<T> entities)
    {
        dbContext.Set<T>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<List<T>> GetPagedReponseAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await dbContext.Set<T>().Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
    }

    public Task UpdateAsync(T entity)
    {
        dbContext.Attach(entity);
        dbContext.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }
}

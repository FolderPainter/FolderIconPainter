using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;
using Domain.Contracts;
using Infrastructure.Contexts;
using System.Collections;

namespace Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly FolderContext dbContext;
    private bool disposed;
    private Hashtable repositories;

    public UnitOfWork(FolderContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task Rollback()
    {
        dbContext.ChangeTracker.Entries().ToList().ForEach(async entity =>
        {
            entity.State = EntityState.Detached;
            await entity.ReloadAsync();
        });
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                //dispose managed resources
                dbContext.Dispose();
            }
        }
        //dispose unmanaged resources
        disposed = true;
    }

    public IRepositoryAsync<TEntity> RepositoryClassic<TEntity>() where TEntity : Entity
    {
        if (repositories == null)
            repositories = new Hashtable();

        var type = typeof(TEntity).Name;

        if (!repositories.ContainsKey(type))
        {
            var repositoryType = typeof(RepositoryAsync<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), dbContext);
            repositories.Add(type, repositoryInstance);
        }

        return (IRepositoryAsync<TEntity>)repositories[type];
    }
}

using Domain.Contracts;

namespace Application.Interfaces.Repositories;
public interface IUnitOfWork : IDisposable
{
    IRepositoryAsync<T> RepositoryClassic<T>() where T : Entity;

    Task<int> CommitAsync(CancellationToken cancellationToken);

    Task Rollback();
}

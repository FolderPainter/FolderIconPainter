using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces.Repositories;
using Infrastructure.Repositories;
using System.Reflection;

namespace Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>))
            .AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
    }
}

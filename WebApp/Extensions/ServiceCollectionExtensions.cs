using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace WebApp.Extensions;
internal static class ServiceCollectionExtensions
{
    internal static async Task<IStringLocalizer> GetRegisteredServerLocalizerAsync<T>(this IServiceCollection services) where T : class
    {
        var serviceProvider = services.BuildServiceProvider();
        var localizer = serviceProvider.GetService<IStringLocalizer<T>>();
        await serviceProvider.DisposeAsync();
        return localizer;
    }

    internal static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<FolderContext>(options => options
                  .UseSqlite(configuration.GetConnectionString("DefaultConnection")));
    }
}

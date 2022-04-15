using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Application.Interfaces.Services;

namespace WebApp.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        internal static async Task<IApplicationBuilder> Initialize(this IApplicationBuilder app, Microsoft.Extensions.Configuration.IConfiguration _configuration)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var initializers = serviceScope.ServiceProvider.GetServices<IDatabaseSeeder>();

            foreach (var initializer in initializers)
            {
                await initializer.InitializeAsync();
            }

            return app;
        }
    }
}

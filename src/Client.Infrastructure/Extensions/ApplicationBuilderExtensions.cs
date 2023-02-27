using Application.Interfaces.Services;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace Server.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task<IApplicationBuilder> InitializeAsync(this IApplicationBuilder app, Microsoft.Extensions.Configuration.IConfiguration _configuration)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<FolderContext>();

        if (context.Database.GetMigrations().Any())
        {
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }
            //await context.Database.MigrateAsync();
            if (await context.Database.CanConnectAsync())
            {
                var initializers = serviceScope.ServiceProvider.GetServices<IDatabaseSeeder>();

                foreach (var initializer in initializers)
                {
                    await initializer.InitializeAsync();
                }
            }
        }

        return app;
    }
}

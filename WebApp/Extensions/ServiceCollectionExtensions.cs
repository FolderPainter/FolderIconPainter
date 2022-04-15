using Application.Interfaces.Services;
using Blazored.LocalStorage;
using Infrastructure;
using Infrastructure.Contexts;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using MudBlazor.Services;
using System.Threading.Tasks;
using WebApp.Services;
using WebApp.Services.UserPreferences;

namespace WebApp.Extensions;
internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<FolderContext>(options => options
            .UseSqlite(configuration.GetConnectionString("DefaultConnection")))
            .AddTransient<IDatabaseSeeder, DatabaseSeeder>();
    }

    internal static IServiceCollection TryAddServices(this IServiceCollection services)
    {
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 10000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });
        services.AddSignalR(e =>
        {
            e.MaximumReceiveMessageSize = 102400000;
        });
        services.AddBlazoredLocalStorage();
        services.AddScoped<IUserPreferencesService, UserPreferencesService>();

        services.AddScoped<LayoutService>();
        services.AddScoped<IconService>();

        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<ICustomFolderService, CustomFolderService>();

        return services;
    }
}

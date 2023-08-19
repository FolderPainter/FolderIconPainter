using FIP.App.Services;
using FIP.Core.Services;
using FIP.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using Windows.Storage;

namespace FIP.App.Helpers
{
    public static class Startup
    {
        public static IHost ConfigureHost()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureLogging(builder => builder
                    .AddProvider(new FileLoggerProvider(Path.Combine(ApplicationData.Current.LocalFolder.Path, "debug.log")))
                    .SetMinimumLevel(LogLevel.Information))
                .ConfigureServices(services => services
                    // Services
                    .AddSingleton<ISVGPainterService, SVGPainterService>()
        
                ).Build();
        }

    }
}

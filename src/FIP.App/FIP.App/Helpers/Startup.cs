using CommunityToolkit.WinUI.Notifications;
using FIP.App.Constants;
using FIP.App.Services;
using FIP.App.ViewModels;
using FIP.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Windows.Storage;
using Windows.UI.Notifications;

namespace FIP.App.Helpers
{
    public static class Startup
    {
        public static IHost ConfigureHost()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices(services => services
                    // Services
                    .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true))
                    .AddSingleton<ISVGPainterService, SVGPainterService>()
                    .AddSingleton<IFolderIconService, FolderIconService>()
                    .AddSingleton<ICategoryStorageService, CategoryStorageService>()
                    .AddSingleton<ICustomIconStorageService, CustomIconStorageService>()
                    // ViewModels
                    .AddSingleton<CustomIconsViewModel>()
                ).Build();
        }

        public static void ConfigureLogger()
        {
            var logPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, AppConstants.StorageSettings.LogsFileName);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(
                new CompactJsonFormatter(), 
                logPath)
                .CreateLogger();
        }

        public static void HandleAppUnhandledException(Exception? ex, bool showToastNotification)
        {
            StringBuilder formattedException = new()
            {
                Capacity = 200
            };

            formattedException.AppendLine("--------- UNHANDLED EXCEPTION ---------");

            if (ex is not null)
            {
                formattedException.AppendLine($">>>> HRESULT: {ex.HResult}");

                if (ex.Message is not null)
                {
                    formattedException.AppendLine("--- MESSAGE ---");
                    formattedException.AppendLine(ex.Message);
                }
                if (ex.StackTrace is not null)
                {
                    formattedException.AppendLine("--- STACKTRACE ---");
                    formattedException.AppendLine(ex.StackTrace);
                }
                if (ex.Source is not null)
                {
                    formattedException.AppendLine("--- SOURCE ---");
                    formattedException.AppendLine(ex.Source);
                }
                if (ex.InnerException is not null)
                {
                    formattedException.AppendLine("--- INNER ---");
                    formattedException.AppendLine(ex.InnerException.ToString());
                }
            }
            else
            {
                formattedException.AppendLine("Exception data is not available.");
            }

            formattedException.AppendLine("---------------------------------------");

            Debug.WriteLine(formattedException.ToString());

            // Please check "Output Window" for exception details (View -> Output Window) (CTRL + ALT + O)
            Debugger.Break();

            App.Logger.LogError(ex, ex?.Message ?? "An unhandled error occurred.");

            if (!showToastNotification)
                return;

            var toastContent = new ToastContent()
            {
                Visual = new()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Something went wrong!"
                            },
                            new AdaptiveText()
                            {
                                Text = "Folder Icon Painter ran into a problem that the developers didn't prepare for yet."
                            }
                        }
                    }
                },
                Actions = new ToastActionsCustom()
                {
                    Buttons =
                    {
                        new ToastButton("Report this issue", AppConstants.GitHub.BugReportUrl)
                        {
                            ActivationType = ToastActivationType.Protocol
                        }
                    }
                },
                ActivationType = ToastActivationType.Protocol
            };

            // Create the toast notification
            var toastNotification = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);

            Process.GetCurrentProcess().Kill();
        }
    }
}

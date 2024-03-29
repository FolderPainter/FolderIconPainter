﻿using CommunityToolkit.Mvvm.DependencyInjection;
using FIP.App.Helpers;
using FIP.App.Views;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using Windows.Globalization;

namespace FIP.App
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static ILogger Logger { get; private set; } = null!;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            UnhandledException += (sender, e) => Startup.HandleAppUnhandledException(e.Exception, true);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => Startup.HandleAppUnhandledException(e.ExceptionObject as Exception, false);
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            m_window = WindowHelper.CreateWindow();

            // Prepare the app shell and window content.
            AppShell shell = m_window.Content as AppShell ?? new AppShell();
            shell.Language = ApplicationLanguages.Languages[0];
            m_window.Content = shell;

            if (shell.AppFrame.Content == null)
            {
                // When the navigation stack isn't restored, navigate to the first page
                // suppressing the initial entrance animation.
                shell.AppFrame.Navigate(typeof(AllIconsPage), null,
                    new SuppressNavigationTransitionInfo());
            }
            m_window.Activate();

            // Configure the DI (dependency injection) container
            var host = Startup.ConfigureHost();
            Startup.ConfigureLogger();
            Ioc.Default.ConfigureServices(host.Services);

            Logger = Ioc.Default.GetRequiredService<ILogger<App>>();
        }

        /// <summary>
        /// Gets main App Window
        /// </summary>
        public static Window Window { get { return m_window; } }
        private static Window m_window;
    }
}

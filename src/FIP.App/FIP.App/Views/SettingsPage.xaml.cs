// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using FIP.App.Helpers;
using FIP.App.Helpers.Settings;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FIP.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public string Version
        {
            get
            {
                return ProcessInfoHelper.GetVersion() is Version version
                    ? string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision)
                    : string.Empty;
            }
        }

        public SettingsPage()
        {
            this.InitializeComponent();
            Loaded += OnSettingsPageLoaded;
        }

        private void OnSettingsPageLoaded(object sender, RoutedEventArgs e)
        {
            var currentTheme = ThemeHelper.RootTheme;
            switch (currentTheme)
            {
                case ElementTheme.Light:
                    themeMode.SelectedIndex = 0;
                    break;
                case ElementTheme.Dark:
                    themeMode.SelectedIndex = 1;
                    break;
                case ElementTheme.Default:
                    themeMode.SelectedIndex = 2;
                    break;
            }
        }

        private void themeMode_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (sender is not UIElement senderUiLement ||
                (themeMode.SelectedItem as ComboBoxItem)?.Tag.ToString() is not string selectedTheme ||
                WindowHelper.GetWindowForElement(this) is not Window window)
            {
                return;
            }

            ThemeHelper.RootTheme = EnumHelper.GetEnum<ElementTheme>(selectedTheme);
            var elementThemeResolved = ThemeHelper.RootTheme == ElementTheme.Default ? ThemeHelper.ActualTheme : ThemeHelper.RootTheme;
            //TitleBarHelper.ApplySystemThemeToCaptionButtons(window, elementThemeResolved);

            //// announce visual change to automation
            //UIHelper.AnnounceActionForAccessibility(
            //    senderUiLement,
            //    $"Theme changed to {elementThemeResolved}",
            //    "ThemeChangedNotificationActivityId");
        }
    }
}

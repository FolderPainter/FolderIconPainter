// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using FIP.App.Views;
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
using Windows.System;
using Windows.UI.ApplicationSettings;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FIP.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppShell : Page
    {
        public AppShell()
        {
            this.InitializeComponent();

            Loaded += (sender, args) =>
            {
                NavView.SelectedItem = CustomerListMenuItem;
            };

            // Set up custom title bar.
            App.Window.ExtendsContentIntoTitleBar = true;
            // Set XAML element as a draggable region.
            App.Window.SetTitleBar(AppTitleBar);

            AppTitle.Text = Windows.ApplicationModel.Package.Current.DisplayName;
        }

        /// <summary>
        /// Gets the navigation frame instance.
        /// </summary>
        public Frame AppFrame => frame;

        /// <summary>
        /// Default keyboard focus movement for any unhandled keyboarding
        /// </summary>
        private void AppShell_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            FocusNavigationDirection direction = FocusNavigationDirection.None;
            switch (e.Key)
            {
                case VirtualKey.Left:
                case VirtualKey.GamepadDPadLeft:
                case VirtualKey.GamepadLeftThumbstickLeft:
                case VirtualKey.NavigationLeft:
                    direction = FocusNavigationDirection.Left;
                    break;
                case VirtualKey.Right:
                case VirtualKey.GamepadDPadRight:
                case VirtualKey.GamepadLeftThumbstickRight:
                case VirtualKey.NavigationRight:
                    direction = FocusNavigationDirection.Right;
                    break;

                case VirtualKey.Up:
                case VirtualKey.GamepadDPadUp:
                case VirtualKey.GamepadLeftThumbstickUp:
                case VirtualKey.NavigationUp:
                    direction = FocusNavigationDirection.Up;
                    break;

                case VirtualKey.Down:
                case VirtualKey.GamepadDPadDown:
                case VirtualKey.GamepadLeftThumbstickDown:
                case VirtualKey.NavigationDown:
                    direction = FocusNavigationDirection.Down;
                    break;
            }

            if (direction != FocusNavigationDirection.None &&
                FocusManager.FindNextFocusableElement(direction) is Control control)
            {
                control.Focus(FocusState.Keyboard);
                e.Handled = true;
            }
        }


        public readonly string CustomerListLabel = "Customer list";

        public readonly string OrderListLabel = "Order list";

        /// <summary>
        /// Navigates to the page corresponding to the tapped item.
        /// </summary>
        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var label = args.InvokedItem as string;
            var pageType =
                args.IsSettingsInvoked ? typeof(SettingsPage) :
                label == CustomerListLabel ? typeof(CreateCustomIconPage) :
                label == OrderListLabel ? typeof(SettingsPage) : null;
            if (pageType != null && pageType != AppFrame.CurrentSourcePageType)
            {
                AppFrame.Navigate(pageType);
            }
        }


        /// <summary>
        /// Ensures the nav menu reflects reality when navigation is triggered outside of
        /// the nav menu buttons.
        /// </summary>
        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                if (e.SourcePageType == typeof(AllIconsPage))
                {
                    NavView.SelectedItem = CustomerListMenuItem;
                }
                else if (e.SourcePageType == typeof(CreateCustomIconPage))
                {
                    NavView.SelectedItem = OrderListMenuItem;
                }
                else if (e.SourcePageType == typeof(SettingsPage))
                {
                    NavView.SelectedItem = NavView.SettingsItem;
                }
            }
        }

        /// <summary>
        /// Invoked when the View Code button is clicked. Launches the repo on GitHub. 
        /// </summary>
        private async void ViewCodeNavPaneButton_Tapped(object sender, TappedRoutedEventArgs e) =>
            await Launcher.LaunchUriAsync(new Uri(
                "https://github.com/Microsoft/Windows-appsample-customers-orders-database"));

        /// <summary>
        /// Navigates the frame to the previous page.
        /// </summary>
        private void NavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (AppFrame.CanGoBack)
            {
                AppFrame.GoBack();
            }
        }
    }
}

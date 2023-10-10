// Licensed under the MIT License.

using FIP.App.Helpers;
using FIP.App.Views;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using System;
using Windows.Storage;
using Windows.System;

namespace FIP.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppShell : Page
    {
        public VirtualKey ArrowKey;
        public Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue;

        public AppShell()
        {
            this.InitializeComponent();

            Loaded += delegate (object sender, RoutedEventArgs e)
            {
                NavigationOrientationHelper.UpdateTitleBarForElement(NavigationOrientationHelper.IsLeftMode(), this);
                
                var window = WindowHelper.GetWindowForElement(sender as UIElement);
                window.Title = AppTitleText;
                window.ExtendsContentIntoTitleBar = true;
                window.Activated += Window_Activated;
                window.SetTitleBar(this.AppTitleBar);
            };
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState == WindowActivationState.Deactivated)
            {
                VisualStateManager.GoToState(this, "Deactivated", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Activated", true);
            }
        }

        // Wraps a call to rootFrame.Navigate to give the Page a way to know which NavigationRootPage is navigating.
        // Please call this function rather than rootFrame.Navigate to navigate the rootFrame.
        public void Navigate(
            Type pageType,
            object targetPageArguments = null,
            Microsoft.UI.Xaml.Media.Animation.NavigationTransitionInfo navigationTransitionInfo = null)
        {
            NavigationRootPageArgs args = new NavigationRootPageArgs();
            args.NavigationRootPage = this;
            args.Parameter = targetPageArguments;
            AppFrame.Navigate(pageType, args, navigationTransitionInfo);
        }

        public static AppShell GetForElement(object obj)
        {
            UIElement element = (UIElement)obj;
            Window window = WindowHelper.GetWindowForElement(element);
            if (window != null)
            {
                return (AppShell)window.Content;
            }
            return null;
        }

        /// <summary>
        /// Gets the navigation frame instance.
        /// </summary>
        public Frame AppFrame => rootFrame;

        public NavigationView NavigationView
        {
            get { return NavigationViewControl; }
        }

        public string AppTitleText
        {
            get
            {
#if DEBUG
                return "Folder Icon Painter Dev";
#else
                return "Folder Icon Painter";
#endif
            }
        }

        public readonly string AllIconsLabel = "All Folder Icons";

        public readonly string CustomIconsLabel = "Manage Custom Icons";

        public readonly string AboutLabel = "About";

        public readonly string SettingsLabel = "Settings";

        /// <summary>
        /// Navigates to the page corresponding to the tapped item.
        /// </summary>
        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            //var label = args.InvokedItem as string;
            //var pageType =
            //    args.IsSettingsInvoked ? typeof(SettingsPage) :
            //    label == AllIconsLabel ? typeof(AllFolderIconsPage) :
            //    label == CustomIconsLabel ? typeof(CustomIconsPage) :
            //    label == AboutLabel ? typeof(AboutPage) : null;
            //if (pageType != null && pageType != AppFrame.CurrentSourcePageType)
            //{
            //    AppFrame.Navigate(pageType);
            //    NavView.Header = label;

            //}
        }

        /// <summary>
        /// Ensures the nav menu reflects reality when navigation is triggered outside of
        /// the nav menu buttons.
        /// </summary>
        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            //if (e.NavigationMode == NavigationMode.Back)
            //{
            //    if (e.SourcePageType == typeof(AllFolderIconsPage))
            //    {
            //        NavView.SelectedItem = AllIconsMenuItem;
            //        NavView.Header = AllIconsLabel;
            //    }
            //    else if (e.SourcePageType == typeof(CustomIconsPage))
            //    {
            //        NavView.SelectedItem = CreateCustomIconMenuItem;
            //        NavView.Header = CustomIconsLabel;
            //    }
            //    else if (e.SourcePageType == typeof(AboutPage))
            //    {
            //        NavView.SelectedItem = AboutMenuItem;
            //        NavView.Header = AboutLabel;

            //    }
            //}
        }

        private void OnNavigationViewSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                if (rootFrame.CurrentSourcePageType != typeof(SettingsPage))
                {
                    Navigate(typeof(SettingsPage));
                    NavigationView.Header = SettingsLabel;
                }
            }
            else
            {
                var selectedItem = args.SelectedItemContainer;
                if (selectedItem == AllIconsItem)
                {
                    if (rootFrame.CurrentSourcePageType != typeof(AllFolderIconsPage))
                    {
                        Navigate(typeof(AllFolderIconsPage));
                        NavigationView.Header = null;
                    }
                }
                else if (selectedItem == CustomIconsItem)
                {
                    if (rootFrame.CurrentSourcePageType != typeof(CustomIconsPage))
                    {
                        Navigate(typeof(CustomIconsPage));
                        NavigationView.Header = CustomIconsLabel;
                    }
                }
                else if (selectedItem == AboutItem)
                {
                    if (rootFrame.CurrentSourcePageType != typeof(AboutPage))
                    {
                        Navigate(typeof(AboutPage));
                        NavigationView.Header = AboutLabel;
                    }
                }
            }
        }

        private void OnRootFrameNavigated(object sender, NavigationEventArgs e)
        {
            //NavView.AlwaysShowHeader = e.SourcePageType != typeof(AllFolderIconsPage);
        }

        /// <summary>
        /// Invoked when the View Code button is clicked. Launches the repo on GitHub. 
        /// </summary>
        private async void ViewCodeNavPaneButton_Tapped(object sender, TappedRoutedEventArgs e) =>
            await Launcher.LaunchUriAsync(new Uri(
                "https://github.com/FolderPainter/FolderIconPainter"));

        /// <summary>
        /// Invoked wgen the Open Icons folder is clicked. Launches the folder
        /// </summary>
        private async void OpenFolderNavPaneButton_Tapped(object sender, TappedRoutedEventArgs e) =>
            await Launcher.LaunchFolderAsync(ApplicationData.Current.LocalFolder);


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

    public class NavigationRootPageArgs
    {
        public AppShell NavigationRootPage;
        public object Parameter;
    }
}

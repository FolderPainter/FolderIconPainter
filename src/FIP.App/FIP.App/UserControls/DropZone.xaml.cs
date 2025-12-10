// Licensed under the MIT License.

using CommunityToolkit.Mvvm.DependencyInjection;
using FIP.App.Helpers;
using FIP.Core.Services;
using FIP.Core.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO.Compression;
using Windows.ApplicationModel.DataTransfer;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI;

namespace FIP.App.UserControls
{
    public sealed partial class DropZone : UserControl
    {
        private IFolderPainterService FolderPainterService { get; } = Ioc.Default.GetRequiredService<IFolderPainterService>();

        public DropZone()
        {
            this.InitializeComponent();
        }

        public static readonly string NoNameString = "No name";

        public static readonly DependencyProperty IconViewModelProperty = 
            DependencyProperty.Register("IconViewModel", typeof(CustomIconViewModel),
                typeof(DropZone), new PropertyMetadata(null));

        public CustomIconViewModel IconViewModel
        {
            get { return (CustomIconViewModel)GetValue(IconViewModelProperty); }
            set { SetValue(IconViewModelProperty, value); }
        }

        public static readonly DependencyProperty ShowIconModeProperty = 
            DependencyProperty.Register("ShowIconMode", typeof(bool),
                typeof(DropZone), new PropertyMetadata(false));

        public bool ShowIconMode
        {
            get { return (bool)GetValue(ShowIconModeProperty); }
            set { SetValue(ShowIconModeProperty, value); }
        }

        public static readonly DependencyProperty ShowNameModeProperty =
            DependencyProperty.Register("ShowNameMode", typeof(bool),
                typeof(DropZone), new PropertyMetadata(false));

        public bool ShowNameMode
        {
            get { return (bool)GetValue(ShowNameModeProperty); }
            set { SetValue(ShowNameModeProperty, value); }
        }

        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Color), 
                typeof(DropZone), new PropertyMetadata(Colors.AliceBlue));

        public Color BackgroundPointerOverColor
        {
            get { return (Color)GetValue(BackgroundPointerOverColorProperty); }
            set { SetValue(BackgroundPointerOverColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundPointerOverColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundPointerOverColorProperty =
            DependencyProperty.Register("BackgroundPointerOverColor", typeof(Color), typeof(DropZone), new PropertyMetadata(Colors.Aqua));

        public Color BackgroundPressedColor
        {
            get { return (Color)GetValue(BackgroundPressedColorProperty); }
            set { SetValue(BackgroundPressedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundPressedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundPressedColorProperty =
            DependencyProperty.Register("BackgroundPressedColor", typeof(Color), typeof(DropZone), new PropertyMetadata(Colors.Aqua));

        private async void DropZoneClick(object sender, RoutedEventArgs e)
        {
            if (IconViewModel is null)
            {
                return;
            }

            // Clear previous returned file name, if it exists, between iterations of this scenario
            ZoneTextBlock.Text = "";

            // Create a folder picker
            FolderPicker openPicker = new FolderPicker();

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var window = WindowHelper.GetWindowForElement(this);
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the folder picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your folder picker
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.FileTypeFilter.Add("*");

            // Open the picker for the user to pick a folder
            StorageFolder folder = await openPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                ZoneTextBlock.Text = "Picked folder: " + folder.Name;

                FolderPainterService.SettingIcon(folder.Path, IconViewModel.IconPath);
                FolderPainterService.RefreshIcons();
            }
            else
            {
                ZoneTextBlock.Text = "Operation cancelled.";
            }
        }

        private async void DropZoneDrop(object sender, DragEventArgs e)
        {
            if (IconViewModel is null)
            {
                return;
            }

            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Count > 0)
                {
                    foreach (var appFile in items)
                    {
                        FolderPainterService.SettingIcon(appFile.Path, IconViewModel.IconPath);
                        FolderPainterService.RefreshIcons();

                        ZoneTextBlock.Text += appFile.Path;
                        ZoneTextBlock.Text += '\n';
                    }
                }
            }
        }

        private void DropZoneDragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;

            if (e.DragUIOverride != null)
            {
                ZoneTextBlock.Text = "Paint Folder!";
            }
        }
    }
}

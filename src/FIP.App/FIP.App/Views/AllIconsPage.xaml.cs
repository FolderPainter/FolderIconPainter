// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using CommunityToolkit.WinUI.Helpers;
using FIP.App.Models;
using FIP.App.UserControls;
using FIP.App.ViewModels;
using FIP.Backend.Helpers;
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
using Windows.UI;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FIP.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllIconsPage : Page
    {
        public string Title { get; set; }

        public AllIconsPage()
        {
            this.InitializeComponent();

            var fipHexColors = DefaultColors.GetAllColors().ToList();
            var dropZoneViewModels = new List<DropZoneViewModel>();

            fipHexColors.ForEach(c => 
            {
                var fipColor = new FIPColor(c);
                var lightnessColor = fipColor.ColorLighten(0.15);
                var darknessColor = fipColor.ColorDarken(0.15);
              
                dropZoneViewModels.Add(
                    new DropZoneViewModel
                    {
                        HexColor = c,
                        BackgroundColor = Color.FromArgb(fipColor.A, fipColor.R, fipColor.G, fipColor.B),
                        BackgroundPointerOverColor = Color.FromArgb(darknessColor.A, darknessColor.R, darknessColor.G, darknessColor.B),
                        BackgroundPressedColor = Color.FromArgb(lightnessColor.A, lightnessColor.R, lightnessColor.G, lightnessColor.B),
                    });
            });

            AdaptiveGridViewControl.ItemsSource = dropZoneViewModels;
        }
    }
}

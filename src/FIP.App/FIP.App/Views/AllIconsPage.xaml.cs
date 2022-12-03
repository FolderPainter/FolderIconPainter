// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using CommunityToolkit.WinUI.Helpers;
using FIP.App.Models;
using FIP.App.UserControls;
using FIP.App.ViewModels;
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
    public sealed partial class AllIconsPage : Page
    {
            public string Title { get; set; }

        public AllIconsPage()
        {
            this.InitializeComponent();

            var colors = DefaultColors.GetAllColors();
            var list = colors.Select(c => new DropZoneViewModel()
            {
                HexColor = c,
                Brush = new SolidColorBrush(ColorHelper.ToColor(c))
            });

            AdaptiveGridViewControl.ItemsSource = list;
        }
    }
}

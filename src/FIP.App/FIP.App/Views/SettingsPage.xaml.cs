// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using FIP.App.Models;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FIP.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public List<ColorGridItem> ColorGridItems { get; set; }

        public FIPColor DefaultFIPColor { get; set; }

        public SettingsPage()
        {
            this.InitializeComponent();

            ColorGridItems = new List<ColorGridItem>();
            DefaultFIPColor = new FIPColor("#FCBC19");

            for (double i = 0; i < 1; i += 0.001)
            {
                ColorGridItems.Add(new ColorGridItem { LightValue = i, LightColor = DefaultFIPColor.ColorLighten(i) });
            }
        }
    }
}

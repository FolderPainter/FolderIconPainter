// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.Graphics.Canvas.Svg;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
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
using Windows.Storage;
using Windows.UI;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using CommunityToolkit.WinUI.Helpers;
using FIP.Backend.Helpers;
using Windows.UI.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FIP.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateCustomIconPage : Page
    {
        CanvasSvgDocument svgDocument;

        Color defaultFolderColor = ColorHelper.ToColor("#FCBC19");

        public Color ContrastColor
        {
            get { return (Color)GetValue(ContrastColorProperty); }
            set { SetValue(ContrastColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContrastColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContrastColorProperty =
            DependencyProperty.Register("ContrastColor", typeof(Color), typeof(CreateCustomIconPage), new PropertyMetadata(Microsoft.UI.Colors.White));

        public CreateCustomIconPage()
        {
            this.InitializeComponent();
            mainColorPicker.Color = defaultFolderColor;
        }

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (svgDocument == null)
            {
                canvasControl.Invalidate();
                return;
            }

            args.DrawingSession.DrawSvg(svgDocument, sender.Size);
        }

        private async void mainColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            await Task.Run(async () =>
            {
                if (svgDocument != null)
                {
                    var fIPColor = new FIPColor(args.NewColor.R, args.NewColor.G, args.NewColor.B, args.NewColor.A);

                    // Fill BackRect 
                    CanvasSvgNamedElement backRect = svgDocument.FindElementById("BackRect");
                    backRect.SetStringAttribute("fill", fIPColor.ToString(ColorOutputFormats.Hex));

                    // New FrontRect_Gradient
                    var frontFirstColor = fIPColor.ColorLighten(0.23);
                    CanvasSvgNamedElement frontGradientFisrtStop = svgDocument.FindElementById("FrontGradientFisrtStop");
                    frontGradientFisrtStop.SetStringAttribute("stop-color", frontFirstColor.ToString(ColorOutputFormats.Hex));

                    var frontSecondColor = fIPColor.ColorLighten(0.07);
                    CanvasSvgNamedElement frontGradientSecondStop = svgDocument.FindElementById("FrontGradientSecondStop");
                    frontGradientSecondStop.SetStringAttribute("stop-color", frontSecondColor.ToString(ColorOutputFormats.Hex));

                    // New FrontDarkRect_Gradient
                    var frontDarkFirstColor = fIPColor.ColorLighten(0.33);
                    CanvasSvgNamedElement frontDarkGradientFisrtStop = svgDocument.FindElementById("FrontDarkGradientFisrtStop");
                    frontDarkGradientFisrtStop.SetStringAttribute("stop-color", frontDarkFirstColor.ToString(ColorOutputFormats.Hex));

                    var frontDarkSecondColor = fIPColor.ColorLighten(0.18);
                    CanvasSvgNamedElement frontDarkGradientSecondStop = svgDocument.FindElementById("FrontDarkGradientSecondStop");
                    frontDarkGradientSecondStop.SetStringAttribute("stop-color", frontDarkSecondColor.ToString(ColorOutputFormats.Hex));

                    // Draw refilled svg image
                    canvasControl.Invalidate();
                }
            });
        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/win11-folder-default.svg"));
            using (var fileStream = await file.OpenReadAsync())
            {
                svgDocument = await CanvasSvgDocument.LoadAsync(canvasControl, fileStream);
                canvasControl.Invalidate();
            }
        }
    }
}

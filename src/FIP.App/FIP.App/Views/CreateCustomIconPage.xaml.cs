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
using CommunityToolkit.WinUI;

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
            // Contrasting button
            var fipColor = new FIPColor(args.NewColor.R, args.NewColor.G, args.NewColor.B, args.NewColor.A);
            if (fipColor.L > .7)
            {
                ContrastColor = Microsoft.UI.Colors.Black;
            }
            else
            {
                ContrastColor = fipColor.H > 19 && fipColor.H < 191 ? 
                    Microsoft.UI.Colors.Black : Microsoft.UI.Colors.White;
            }

            await Task.Run(async Task<bool> () =>
            {
                if (svgDocument != null)
                {
                    // Fill BackRect 
                    CanvasSvgNamedElement backRect = svgDocument.FindElementById("BackRect");
                    backRect.SetStringAttribute("fill", fipColor.ToString(ColorOutputFormats.Hex));

                    // New FrontRect_Gradient
                    var frontFirstColor = fipColor.ColorLighten(0.23);
                    CanvasSvgNamedElement frontGradientFisrtStop = svgDocument.FindElementById("FrontGradientFisrtStop");
                    frontGradientFisrtStop.SetStringAttribute("stop-color", frontFirstColor.ToString(ColorOutputFormats.Hex));

                    var frontSecondColor = fipColor.ColorLighten(0.07);
                    CanvasSvgNamedElement frontGradientSecondStop = svgDocument.FindElementById("FrontGradientSecondStop");
                    frontGradientSecondStop.SetStringAttribute("stop-color", frontSecondColor.ToString(ColorOutputFormats.Hex));

                    // New FrontDarkRect_Gradient
                    var frontDarkFirstColor = fipColor.ColorLighten(0.33);
                    CanvasSvgNamedElement frontDarkGradientFisrtStop = svgDocument.FindElementById("FrontDarkGradientFisrtStop");
                    frontDarkGradientFisrtStop.SetStringAttribute("stop-color", frontDarkFirstColor.ToString(ColorOutputFormats.Hex));

                    var frontDarkSecondColor = fipColor.ColorLighten(0.18);
                    CanvasSvgNamedElement frontDarkGradientSecondStop = svgDocument.FindElementById("FrontDarkGradientSecondStop");
                    frontDarkGradientSecondStop.SetStringAttribute("stop-color", frontDarkSecondColor.ToString(ColorOutputFormats.Hex));

                    // Draw refilled svg image
                    canvasControl.Invalidate();
                }
                return await Task.FromResult(true);
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

    public class ColorToHslStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Color color = (Color)value;
            HslColor hslColor = color.ToHsl();
            if (parameter!= null)
            {
                switch (parameter)
                {
                    case "h":
                        break;
                    case "s":
                        break;
                    case "l":
                        break;
                    default:
                        break;
                }
            }
            return $"{hslColor.H} | {hslColor.S} | {hslColor.L}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return ColorHelper.ToColor(((HslColor)value).ToString());
        }
    }
}

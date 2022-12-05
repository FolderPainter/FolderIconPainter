// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.Graphics.Canvas.Svg;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using Windows.Storage;
using Windows.UI;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.Helpers;
using FIP.Backend.Helpers;
using CommunityToolkit.WinUI;
using System.IO;
using System.Text;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.UI;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Microsoft.UI.Xaml.Media;
using Windows.Storage.Streams;
using Svg;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Imaging;
using FIP.App.Helpers;

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

        Color defaultFolderColor = CommunityToolkit.WinUI.Helpers.ColorHelper.ToColor("#04805C");

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

            var tmpc = new FIPColor(defaultFolderColor.R, defaultFolderColor.G, defaultFolderColor.B, defaultFolderColor.A);
            BackFirstOG.Text = tmpc.ToString(ColorOutputFormats.HSL);
            BackSecondOG.Text = new FIPColor("#0A5D5E").ToString(ColorOutputFormats.HSL);

            MiddleFirstOG.Text = new FIPColor("#70DAB1").ToString(ColorOutputFormats.HSL);
            MiddleSecondOG.Text = new FIPColor("#1AB19B").ToString(ColorOutputFormats.HSL);

            FrontFirstOG.Text = new FIPColor("#16C997").ToString(ColorOutputFormats.HSL);
            FrontSecondOG.Text = new FIPColor("#0099A6").ToString(ColorOutputFormats.HSL);
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
            FIPColor backSecondColor = fipColor, middleFirstColor = fipColor, middleSecondColor = fipColor, frontFirstColor = fipColor, frontSecondColor = fipColor;

            await Task.Run(async Task<bool> () =>
            {
                if (svgDocument != null)
                {
                    // Refill BackRect Gradient 
                    CanvasSvgNamedElement backGradientFirstStop = svgDocument.FindElementById("BackGradientFirstStop");
                    backGradientFirstStop.SetStringAttribute("stop-color", fipColor.ToString(ColorOutputFormats.Hex));

                    backSecondColor = fipColor.ChangeSL(-0.13, -0.06);
                    CanvasSvgNamedElement backGradientSecondStop = svgDocument.FindElementById("BackGradientSecondStop");
                    backGradientSecondStop.SetStringAttribute("stop-color", backSecondColor.ToString(ColorOutputFormats.Hex));

                    // Refill MiddleRect Gradient 
                    middleFirstColor = fipColor.ChangeSL(-0.35, 0.39);
                    CanvasSvgNamedElement middleGradientFirstStop = svgDocument.FindElementById("MiddleGradientFirstStop");
                    middleGradientFirstStop.SetStringAttribute("stop-color", middleFirstColor.ToString(ColorOutputFormats.Hex));

                    middleSecondColor = fipColor.ChangeSL(-0.2, 0.14);
                    CanvasSvgNamedElement middleGradientSecondStop = svgDocument.FindElementById("MiddleGradientSecondStop");
                    middleGradientSecondStop.SetStringAttribute("stop-color", middleSecondColor.ToString(ColorOutputFormats.Hex));

                    // Refill FrontRect Gradient 
                    frontFirstColor = fipColor.ChangeSL(-0.14, 0.18);
                    CanvasSvgNamedElement frontGradientFirstStop = svgDocument.FindElementById("FrontGradientFirstStop");
                    frontGradientFirstStop.SetStringAttribute("stop-color", frontFirstColor.ToString(ColorOutputFormats.Hex));

                    frontSecondColor = fipColor.ChangeSL(0.06, 0.07);
                    CanvasSvgNamedElement frontGradientSecondStop = svgDocument.FindElementById("FrontGradientSecondStop");
                    frontGradientSecondStop.SetStringAttribute("stop-color", frontSecondColor.ToString(ColorOutputFormats.Hex));

                    // Draw refilled svg image
                    canvasControl.Invalidate();

                   
                }
                return await Task.FromResult(true);
            });

            //Test 
            BackFirst.Text = fipColor.ToString(ColorOutputFormats.HSL);
            BackSecond.Text = backSecondColor.ToString(ColorOutputFormats.HSL);

            MiddleFirst.Text = middleFirstColor.ToString(ColorOutputFormats.HSL);
            MiddleSecond.Text = middleSecondColor.ToString(ColorOutputFormats.HSL);

            FrontFirst.Text = frontFirstColor.ToString(ColorOutputFormats.HSL);
            FrontSecond.Text = frontSecondColor.ToString(ColorOutputFormats.HSL);
        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {

        }

        private async void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (svgDocument == null)
                return;

            var svgString = "";
            using (var stream = new MemoryStream())
            {
                await svgDocument.SaveAsync(stream.AsRandomAccessStream());

                stream.Position = 0;
                var streamReader = new StreamReader(stream, Encoding.UTF8);
                svgString = streamReader.ReadToEnd();
            }

            SvgDocument svg = SvgDocument.FromSvg<SvgDocument>(svgString);
            svg.ShapeRendering = SvgShapeRendering.Auto;
            System.Drawing.Bitmap bitmap = svg.Draw(256, 0);

            StorageFolder storageFolder = KnownFolders.SavedPictures;
            StorageFile storageFile = await storageFolder.CreateFileAsync($"sample{new Random().Next()}.ico");
            await ImageHelper.SaveBitmapAsIconAsync(bitmap, storageFile.Path);
        }

        CanvasVirtualBitmap virtualBitmap;

        private async void OnSaveAsClicked(object sender, RoutedEventArgs e)
        {
            using (var device = CanvasDevice.GetSharedDevice())
            using (var renderTarget = new CanvasRenderTarget(device, 512, 512, 96))
            {
                using (var ds = renderTarget.CreateDrawingSession())
                {
                    ds.DrawText("Win2D", 512 / 2, 512 / 2, Colors.White,
                    new CanvasTextFormat()
                    {
                        VerticalAlignment = CanvasVerticalAlignment.Center,
                        HorizontalAlignment = CanvasHorizontalAlignment.Center,
                        FontFamily = "Comic Sans MS",
                        FontSize = (float)(512 / 4)
                    });
                }
                Random random = new Random();
                StorageFolder storageFolder = KnownFolders.SavedPictures;
                var file = await storageFolder.CreateFileAsync($"sample{random.Next()}.jpg", CreationCollisionOption.ReplaceExisting);

                using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await renderTarget.SaveAsync(fileStream, CanvasBitmapFileFormat.Png);
                }
            }
        }

        private async void canvasControl_CreateResources(CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/win11-folder-green.svg"));
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
            return $"{hslColor.H} | {hslColor.S} | {hslColor.L}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return CommunityToolkit.WinUI.Helpers.ColorHelper.ToColor(((HslColor)value).ToString());
        }
    }
}

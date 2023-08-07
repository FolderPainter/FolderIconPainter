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
using Microsoft.UI;
using Svg;
using FIP.App.Helpers;
using System.Drawing.Drawing2D;

namespace FIP.App.Views
{
    /// <summary>
    /// Page for create custom icon
    /// </summary>
    public sealed partial class CreateCustomIconPage : Page
    {
        CanvasSvgDocument canvasSVG;

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

            MiddleFirstOG.Text = new FIPColor("#5CD1AC").ToString(ColorOutputFormats.HSL);
            MiddleSecondOG.Text = new FIPColor("#1AB19B").ToString(ColorOutputFormats.HSL);

            FrontFirstOG.Text = new FIPColor("#16C997").ToString(ColorOutputFormats.HSL);
            FrontSecondOG.Text = new FIPColor("#0099A6").ToString(ColorOutputFormats.HSL);
        }

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (canvasSVG == null)
            {
                canvasControl.Invalidate();
                return;
            }

            args.DrawingSession.DrawSvg(canvasSVG, sender.Size);
        }

        private async void mainColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            // Contrasting button
            var fipColor = new FIPColor(args.NewColor.R, args.NewColor.G, args.NewColor.B, args.NewColor.A);
            if (fipColor.L > .7)
            {
                ContrastColor = Colors.Black;
            }
            else
            {
                ContrastColor = fipColor.H > 19 && fipColor.H < 191 ?
                    Colors.Black : Colors.White;
            }
            FIPColor backSecondColor = fipColor, middleFirstColor = fipColor, middleSecondColor = fipColor, frontFirstColor = fipColor, frontSecondColor = fipColor;

            await Task.Run(async Task<bool> () =>
            {
                if (canvasSVG != null)
                {
                    // Refill BackRect Gradient 
                    CanvasSvgNamedElement backGradientFirstStop = canvasSVG.FindElementById("BackGradientFirstStop");
                    backGradientFirstStop.SetStringAttribute("stop-color", fipColor.ToString(ColorOutputFormats.Hex));

                    backSecondColor = fipColor.ChangeSL(-0.13, -0.06);
                    CanvasSvgNamedElement backGradientSecondStop = canvasSVG.FindElementById("BackGradientSecondStop");
                    backGradientSecondStop.SetStringAttribute("stop-color", backSecondColor.ToString(ColorOutputFormats.Hex));

                    // Refill MiddleRect Gradient 
                    middleFirstColor = fipColor.ChangeSL(-0.38, 0.33);
                    CanvasSvgNamedElement middleGradientFirstStop = canvasSVG.FindElementById("MiddleGradientFirstStop");
                    middleGradientFirstStop.SetStringAttribute("stop-color", middleFirstColor.ToString(ColorOutputFormats.Hex));

                    middleSecondColor = fipColor.ChangeSL(-0.2, 0.14);
                    CanvasSvgNamedElement middleGradientSecondStop = canvasSVG.FindElementById("MiddleGradientSecondStop");
                    middleGradientSecondStop.SetStringAttribute("stop-color", middleSecondColor.ToString(ColorOutputFormats.Hex));

                    // Refill FrontRect Gradient 
                    frontFirstColor = fipColor.ChangeSL(-0.14, 0.18);
                    CanvasSvgNamedElement frontGradientFirstStop = canvasSVG.FindElementById("FrontGradientFirstStop");
                    frontGradientFirstStop.SetStringAttribute("stop-color", frontFirstColor.ToString(ColorOutputFormats.Hex));

                    frontSecondColor = fipColor.ChangeSL(0.06, 0.07);
                    CanvasSvgNamedElement frontGradientSecondStop = canvasSVG.FindElementById("FrontGradientSecondStop");
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

        // Saves created image
        private async void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (canvasSVG == null)
                return;

            var svgString = "";
            using (var stream = new MemoryStream())
            {
                await canvasSVG.SaveAsync(stream.AsRandomAccessStream());

                stream.Position = 0;
                var streamReader = new StreamReader(stream, Encoding.UTF8);
                svgString = streamReader.ReadToEnd();
            }

            SvgDocument newSVG = SvgDocument.FromSvg<SvgDocument>(svgString);
            newSVG.ShapeRendering = SvgShapeRendering.Auto;
            newSVG.Ppi = 72;
            System.Drawing.Bitmap svgBitmap = newSVG.Draw(256, 256);
            var svgBitmapGraphics = System.Drawing.Graphics.FromImage(svgBitmap);
            svgBitmapGraphics.SmoothingMode = SmoothingMode.Default;
            svgBitmapGraphics.DrawImage(svgBitmap, 0, 0, 256, 256);

            StorageFolder iconsFolder = KnownFolders.SavedPictures;
            StorageFile newIconFile = await iconsFolder.CreateFileAsync($"{NameBox.Text}.ico");
            await ImageHelper.SaveBitmapAsIconAsync(svgBitmap, newIconFile.Path);
        }

        private async void canvasControl_CreateResources(CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            var svgFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/win11-folder-green2.svg"));
            using (var fileStream = await svgFile.OpenReadAsync())
            {
                canvasSVG = await CanvasSvgDocument.LoadAsync(canvasControl, fileStream);
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

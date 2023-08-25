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
using CommunityToolkit.WinUI;
using System.IO;
using System.Text;
using Microsoft.UI;
using Svg;
using FIP.App.Helpers;
using System.Drawing.Drawing2D;
using FIP.Core.Models;
using FIP.Core.Services;
using CommunityToolkit.Mvvm.DependencyInjection;
using FIP.App.Constants;
using FIP.App.ViewModels;
using System.Collections.Generic;
using FIP.Core.ViewModels;
using System.Linq;

namespace FIP.App.Views
{
    /// <summary>
    /// Page for create custom icon
    /// </summary>
    public sealed partial class CreateCustomIconPage : Page
    {
        // Dependency injections
        private ISVGPainterService SVGPainterService { get; } = Ioc.Default.GetRequiredService<ISVGPainterService>();
        private ICategoryStorageService CategoryStorageService { get; } = Ioc.Default.GetRequiredService<ICategoryStorageService>();
        private ICustomIconStorageService CustomIconStorageService { get; } = Ioc.Default.GetRequiredService<ICustomIconStorageService>();

        private CreateCustomIconViewModel ViewModel { get; } = Ioc.Default.GetRequiredService<CreateCustomIconViewModel>();


        CanvasSvgDocument canvasSVG;

        Color defaultFolderColor = CommunityToolkit.WinUI.Helpers.ColorHelper.ToColor("#04805C");

        public Color ButtonTitleColor
        {
            get { return (Color)GetValue(ButtonTitleColorProperty); }
            set { SetValue(ButtonTitleColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonTitleColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonTitleColorProperty =
            DependencyProperty.Register("ButtonTitleColor", typeof(Color), typeof(CreateCustomIconPage), new PropertyMetadata(Colors.White));

        public CreateCustomIconPage()
        {
            this.InitializeComponent();

            mainColorPicker.Color = defaultFolderColor;
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
            var colorFromPicker = new FIPColor(args.NewColor.R, args.NewColor.G, args.NewColor.B, args.NewColor.A);

            SetUpButtonTitleColor(colorFromPicker);

            await Task.Run(async Task<bool> () =>
            {
                if (canvasSVG != null)
                {
                    // Repaint SVG gradients
                    SVGPainterService.ApplyColorPalette(colorFromPicker);

                    // Draw refilled svg image
                    canvasControl.Invalidate();
                }
                return await Task.FromResult(true);
            });
        }

        // Saves created image
        private async void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (canvasSVG == null)
                return;

            //.getxml();
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
            StorageFile newIconFile = await iconsFolder.CreateFileAsync($"{ViewModel.NewCustomIcon.Name}.ico");
            await ImageHelper.SaveBitmapAsIconAsync(svgBitmap, newIconFile.Path);
        }

        private async void canvasControl_CreateResources(CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            var svgFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(AppConstants.AssetPaths.SVGFolderIconTemplate));
            using (var fileStream = await svgFile.OpenReadAsync())
            {
                canvasSVG = await CanvasSvgDocument.LoadAsync(canvasControl, fileStream);
                SVGPainterService.Initialize(canvasSVG);
                canvasControl.Invalidate();
            }
        }

        /// <summary>
        /// Sets contrasted button font color by picked color
        /// </summary>
        /// <param name="colorFromPicker">Selected color from picker</param>
        private void SetUpButtonTitleColor(FIPColor colorFromPicker) 
        {
            if (colorFromPicker is null)
            {
                ButtonTitleColor = Colors.Black;
                return;
            }

            if (colorFromPicker.L > .7)
            {
                ButtonTitleColor = Colors.Black;
            }
            else
            {
                ButtonTitleColor = colorFromPicker.H > 19 && colorFromPicker.H < 191 ?
                    Colors.Black : Colors.White;
            }
        }

        private void CategorySearchBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var querySplit = sender.Text.ToLower().Split(" ");
                var suggestions = CategoryStorageService.Categories.Where(
                        item =>
                        {
                            bool flag = true;
                            foreach (string queryToken in querySplit)
                            {
                                // Check if token is not in string
                                if (item.Name.ToLower().IndexOf(queryToken, StringComparison.CurrentCultureIgnoreCase) < 0)
                                {
                                    // Token is not in string, so we ignore this item.
                                    flag = false;
                                }
                            }
                            return flag;
                        });
                 
                if (suggestions.Count() > 0)
                {
                    CategorySearchBox.ItemsSource = suggestions.OrderByDescending(i => i.Name.StartsWith(sender.Text, StringComparison.CurrentCultureIgnoreCase)).ThenBy(i => i.Name);
                }
                else
                {
                    CategorySearchBox.ItemsSource = new [] { new CategoryViewModel { Name = sender.Text } };
                }
            }
        }

        private void CategorySearchBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null && args.ChosenSuggestion is CategoryViewModel)
            {
                var category = args.ChosenSuggestion as CategoryViewModel;
            }
        }

        private void CategorySearchBoxSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem != null && args.SelectedItem is CategoryViewModel)
            {
                var category = args.SelectedItem as CategoryViewModel;

                ViewModel.CurrentCategory = category;
            }
        }
    }
}

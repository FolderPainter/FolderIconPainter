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
using System.Collections.ObjectModel;
using Bitmap = System.Drawing.Bitmap;

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

        ObservableCollection<CustomIconViewModel> CustomIconViewModels = new ObservableCollection<CustomIconViewModel>();

        CanvasSvgDocument canvasSVG;

        Color defaultFolderColor = CommunityToolkit.WinUI.Helpers.ColorHelper.ToColor(AppConstants.ColorSettings.DefaultFolderColor);

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
            InitializeComponent();

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
                    
                    ViewModel.NewCustomIcon.Color = args.NewColor.ToHex();  

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

            var svgString = canvasSVG.GetXml();

            SvgDocument newSVG = SvgDocument.FromSvg<SvgDocument>(svgString);
            var svgBitmap = SaveSvgDocumentToBitmap(newSVG);

            StorageFolder iconsFolder = KnownFolders.SavedPictures;
            StorageFile newIconFile = await iconsFolder.CreateFileAsync($"{ViewModel.NewCustomIcon.Name}.ico");
            bool iconCreated = await ImageHelper.SaveBitmapAsIconAsync(svgBitmap, newIconFile.Path);

            if (iconCreated)
            {
                Guid categoryId = Guid.NewGuid();

                ViewModel.CurrentCategory.Id = categoryId;
                CategoryStorageService.AddCategory(ViewModel.CurrentCategory);

                ViewModel.NewCustomIcon.CategoryId = categoryId;
                ViewModel.NewCustomIcon.Id = Guid.NewGuid();
                CustomIconStorageService.AddCustomIcon(ViewModel.NewCustomIcon);
            }
        }

        private Bitmap SaveSvgDocumentToBitmap(SvgDocument svgDocument)
        {
            if (svgDocument is null)
                return new Bitmap(0, 0);

            svgDocument.ShapeRendering = SvgShapeRendering.Auto;
            svgDocument.Ppi = 72;
            Bitmap svgBitmap = svgDocument.Draw(256, 256);

            var svgBitmapGraphics = System.Drawing.Graphics.FromImage(svgBitmap);
            svgBitmapGraphics.SmoothingMode = SmoothingMode.Default;
            svgBitmapGraphics.DrawImage(svgBitmap, 0, 0, 256, 256);

            return svgBitmap;
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

            if (colorFromPicker.L > AppConstants.ColorSettings.MinContrastLightness)
            {
                ButtonTitleColor = Colors.Black;
            }
            else
            {
                ButtonTitleColor = colorFromPicker.H > AppConstants.ColorSettings.MinContrastHueAngle &&
                    colorFromPicker.H < AppConstants.ColorSettings.MaxContrastHueAngle ?
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
                    CategorySearchBox.ItemsSource = new[] { new CategoryViewModel { Name = sender.Text } };
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

                CustomIconViewModels.Clear();

                var icons = CustomIconStorageService.GetCustomIconsByCategoryId(category.Id);
                foreach (var icon in icons)
                {
                    CustomIconViewModels.Add(icon);
                }
            }
        }
    }
}

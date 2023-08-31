using Microsoft.Graphics.Canvas.Svg;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage;
using Windows.UI;
using System.Threading.Tasks;
using Microsoft.UI;
using FIP.Core.Models;
using FIP.Core.Services;
using CommunityToolkit.Mvvm.DependencyInjection;
using FIP.App.Constants;
using FIP.App.ViewModels;
using FIP.Core.ViewModels;
using System.Linq;
using Microsoft.UI.Xaml.Navigation;

namespace FIP.App.Views
{
    /// <summary>
    /// Page for create custom icon
    /// </summary>
    public sealed partial class CreateCustomIconPage : Page
    {
        // Dependency injections
        private ISVGPainterService SVGPainterService { get; } = Ioc.Default.GetRequiredService<ISVGPainterService>();

        private CreateCustomIconViewModel ViewModel { get; } = Ioc.Default.GetRequiredService<CreateCustomIconViewModel>();

        Color defaultFolderColor = CommunityToolkit.WinUI.Helpers.ColorHelper.ToColor(AppConstants.ColorSettings.DefaultFolderColor);

        public CreateCustomIconPage()
        {
            InitializeComponent();

            mainColorPicker.Color = defaultFolderColor;
            ViewModel.NewCustomIcon.Color = AppConstants.ColorSettings.DefaultFolderColor;
            CategorySearchBox.ItemsSource = ViewModel.Categories;
        }

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (ViewModel.CanvasSVG == null)
            {
                canvasControl.Invalidate();
                return;
            }

            args.DrawingSession.DrawSvg(ViewModel.CanvasSVG, sender.Size);
        }

        private async void mainColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            var colorFromPicker = new FIPColor(args.NewColor);

            SetUpButtonTitleColor(colorFromPicker);

            await Task.Run(async Task<bool> () =>
            {
                if (ViewModel.CanvasSVG != null)
                {
                    // Repaint SVG gradients
                    SVGPainterService.ApplyColorPalette(colorFromPicker);

                    ViewModel.NewCustomIcon.Color = colorFromPicker.ToString(ColorOutputFormats.Hex);

                    // Draw refilled svg image
                    canvasControl.Invalidate();
                }
                return await Task.FromResult(true);
            });
        }

        // Saves created image
        private async void createButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.CreateCustomIconAsync();
            CategorySearchBox.Text = ViewModel.CurrentCategory.Name;
        }

        private async void canvasControl_CreateResources(CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            var svgFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(AppConstants.AssetPaths.SVGFolderIconTemplate));
            using (var fileStream = await svgFile.OpenReadAsync())
            {
                ViewModel.CanvasSVG = await CanvasSvgDocument.LoadAsync(canvasControl, fileStream);
                SVGPainterService.Initialize(ViewModel.CanvasSVG);
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
                ViewModel.ButtonTitleColor = Colors.Black;
                return;
            }

            if (colorFromPicker.L > AppConstants.ColorSettings.MinContrastLightness)
            {
                ViewModel.ButtonTitleColor = Colors.Black;
            }
            else
            {
                ViewModel.ButtonTitleColor = colorFromPicker.H > AppConstants.ColorSettings.MinContrastHueAngle &&
                    colorFromPicker.H < AppConstants.ColorSettings.MaxContrastHueAngle ?
                    Colors.Black : Colors.White;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.CurrentCategory = new CategoryViewModel(new Category { Id = Guid.Empty });
            ViewModel.RefreshCustomIconViewModels();
        }

        private void CategorySearchBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (String.IsNullOrEmpty(sender.Text))
                {
                    sender.ItemsSource = null;
                    ViewModel.CurrentCategory = new CategoryViewModel(new Category { Id = Guid.Empty });
                    return;
                }

                var querySplit = sender.Text.ToLower().Split(" ");
                var suggestions = ViewModel.Categories
                    .Where(item => querySplit.All(queryToken => item.Name?.IndexOf(queryToken, StringComparison.CurrentCultureIgnoreCase) >= 0))
                    .Select(category => new CategoryViewModel(category));

                if (!suggestions.Any(s => s.Name == sender.Text))
                {
                    var category = new CategoryViewModel(new Category { Name = sender.Text });
                    category.IsNewCategory = true;
                    suggestions = suggestions.Append(category);
                }

                suggestions = suggestions.OrderByDescending(i => i.Name.StartsWith(sender.Text, StringComparison.CurrentCultureIgnoreCase)).ThenBy(i => i.Name);

                sender.ItemsSource = suggestions;
            }
        }

        private void CategorySearchBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null && args.ChosenSuggestion is CategoryViewModel)
            {
                try
                {
                    var category = args.ChosenSuggestion as CategoryViewModel;
                    ViewModel.CurrentCategory = category;

                    if (ViewModel.CurrentCategory.IsNewCategory)
                    {
                        ViewModel.SelectedCustomIcon = null;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private void IconsGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is GridView gridView)
            {
                if (gridView.SelectedItems.Count > 0)
                {
                    ViewModel.SelectedCustomIcon = gridView.SelectedItems.Select(i => i as CustomIconViewModel).ToList();
                }
                else
                {
                    ViewModel.SelectedCustomIcon = null;
                }
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ViewModel.DeleteSelectedCustomIcons();
            }
            catch (Exception ex)
            {
                var dialog = new ContentDialog()
                {
                    Title = "Unable to delete order",
                    Content = $"There was an error when we tried to delete " +
                        $"invoice #:\n{ex.Message}",
                    PrimaryButtonText = "OK"
                };
                dialog.XamlRoot = App.Window.Content.XamlRoot;
                await dialog.ShowAsync();
            }

        }
    }
}

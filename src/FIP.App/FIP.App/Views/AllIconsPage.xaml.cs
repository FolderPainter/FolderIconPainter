using FIP.App.Constants;
using FIP.App.ViewModels;
using FIP.Backend.Helpers;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;

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

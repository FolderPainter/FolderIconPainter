using Windows.UI;
using FIP.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using FIP.Core.ViewModels;
using FIP.Core.Models;

namespace FIP.App.ViewModels
{
    public class DropZoneViewModel : ObservableObject
    {
        public DropZoneViewModel(CustomIconViewModel customIcon)
        {
            var fipColor = new FIPColor(customIcon.Color);
            CustomIconVM = customIcon;

            BackgroundColor = fipColor.ToWindowsUIColor();
            BackgroundPointerOverColor = fipColor.ColorLighten(0.15).ToWindowsUIColor();
            BackgroundPressedColor = fipColor.ColorDarken(0.15).ToWindowsUIColor();
        }

        public CustomIconViewModel CustomIconVM { get; protected set; }

        public string HexColor => CustomIconVM?.Color;

        public Color BackgroundColor { get; set; }

        public Color BackgroundPointerOverColor { get; set; }

        public Color BackgroundPressedColor { get; set; }
    }
}

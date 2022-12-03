using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace FIP.App.ViewModels
{
    public class DropZoneViewModel
    {
        public string HexColor { get; set; }

        public Color BackgroundColor { get; set; }

        public Color BackgroundPointerOverColor { get; set; }

        public Color BackgroundPressedColor { get; set; }
    }
}

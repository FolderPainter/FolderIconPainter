using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIP.App.Constants;

public class SVGGradientPalette
{
    public string GradientStopIdentifier { get; set; } = "Stop";

    public string GradientName { get; set; }

    public List<PaletteUnit> PaletteUnits { get; set; }

}

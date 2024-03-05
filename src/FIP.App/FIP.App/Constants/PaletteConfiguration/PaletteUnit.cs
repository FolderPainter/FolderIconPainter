namespace FIP.App.Constants;

public class PaletteUnit
{
    public int Number { get; set; }

    // Represents the hue angle
    public double H { get; set; } = 0;

    // Represents saturation
    public double S { get; set; } = 0;

    // Represents Lightness 
    public double L { get; set; } = 0;
}

public static class PaletteUnitExtensions
{
    public static bool IsMainColor(this PaletteUnit paletteUnit)
    {
        return paletteUnit.H.Equals(0.0) 
            && paletteUnit.S.Equals(0.0) 
            && paletteUnit.L.Equals(0.0);
    }
}

namespace FIP.App.Constants;

public static class PaletteConfiguration
{
    public readonly static SVGGradientPalette BackRectGradient = new()
    {
        GradientName = "BackGradient",
        PaletteUnits = new()
        {
            new PaletteUnit
            {
                Number = 0
            },
            new PaletteUnit
            {
                Number = 1,
                H = -1,
                S = -0.11,
                L = -0.01
            },
            new PaletteUnit
            {
                Number = 2,
                H = -2,
                S = -0.2,
                L = -0.01
            },
            new PaletteUnit
            {
                Number = 3,
                H = -3,
                S = -0.23,
                L = -0.01
            }
        }
    };

    public readonly static SVGGradientPalette ShadowGradient = new()
    {
        GradientName = "ShadowGradient",
        PaletteUnits = new()
        {
            new PaletteUnit
            {
                Number = 1
            }
        }
    };

    public readonly static SVGGradientPalette MiddleRectGradient = new()
    {
        GradientName = "MiddleGradient",
        PaletteUnits = new()
        {
            new PaletteUnit
            {
                Number = 0,
                H = 2,
                S = 0.03,
                L = 0.33
            },
            new PaletteUnit
            {
                Number = 1,
                H = 1,
                S = 0.02,
                L = 0.18
            }
        }
    };

    public readonly static SVGGradientPalette FrontRectGradient = new()
    {
        GradientName = "FrontGradient",
        PaletteUnits = new()
        {
            new PaletteUnit
            {
                Number = 0,
                H = 1,
                S = 0.03,
                L = 0.23
            },
            new PaletteUnit
            {
                Number = 1,
                H = 1,
                S = 0.03,
                L = 0.18
            },
            new PaletteUnit
            {
                Number = 2,
                H = 1,
                S = 0.03,
                L = 0.07
            }
        }
    };

}

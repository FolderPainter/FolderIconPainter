using FIP.App.Constants;
using FIP.App.Models;
using Microsoft.Graphics.Canvas.Svg;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FIP.App.Helpers
{
    public class SVGColorHelper
    {
        private CanvasSvgDocument canvasSVG;

        public async void ApplyColorPalette(CanvasSvgDocument canvasSVG, FIPColor color)
        {
            await Task.Run(() =>
            {
                try
                {
                    this.canvasSVG = canvasSVG;

                    var gradients = typeof(PaletteConfiguration).GetFields(BindingFlags.Public | BindingFlags.Static)
                              .Where(f => f.FieldType == typeof(SVGGradientPalette))
                              .Select(f => (SVGGradientPalette)f.GetValue(null));

                    Parallel.ForEach(gradients, gradient =>
                    {
                        ApplyColorPaletteForGradient(gradient, color);
                    });
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            });
        }

        private void ApplyColorPaletteForGradient(SVGGradientPalette gradient, FIPColor mainColor)
        {
            string gradientName = $"{gradient.GradientName}{gradient.GradientStopIdentifier}";
            Parallel.ForEach(gradient.PaletteUnits, paletteUnit =>
            {
                SetColorForStop(gradientName, paletteUnit, mainColor);
            });
        }

        private void SetColorForStop(string gradientName, PaletteUnit paletteUnit, FIPColor mainColor)
        {
            if (paletteUnit.IsMainColor())
                mainColor = mainColor.ChangeHSL(paletteUnit.H, paletteUnit.S, paletteUnit.L);

            CanvasSvgNamedElement frontGradientSecondStop = canvasSVG.FindElementById($"{gradientName}{paletteUnit.Number}");
            frontGradientSecondStop.SetStringAttribute("stop-color", mainColor.ToString(ColorOutputFormats.Hex));
        }
    }
}

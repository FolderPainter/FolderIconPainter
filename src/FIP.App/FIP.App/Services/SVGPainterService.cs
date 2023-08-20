using FIP.App.Constants;
using FIP.Core.Models;
using FIP.Core.Services;
using Microsoft.Graphics.Canvas.Svg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FIP.App.Services
{
    public class SVGPainterService : ISVGPainterService
    {
        public CanvasSvgDocument CanvasSVG { get; set; }

        private IEnumerable<SVGGradientPalette> SVGGradientPalettes { get; set; }

        public void Initialize(CanvasSvgDocument canvasSVG)
        {
            if (canvasSVG != null)
            {
                CanvasSVG = canvasSVG;

                try
                {
                    SVGGradientPalettes = typeof(PaletteConfiguration).GetFields(BindingFlags.Public | BindingFlags.Static)
                             .Where(f => f.FieldType == typeof(SVGGradientPalette))
                             .Select(f => (SVGGradientPalette)f.GetValue(null));
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            else
            {
                throw new Exception("SVG canvas is null.");
            }
        }

        public async void ApplyColorPalette(FIPColor color)
        {
            await Task.Run(() =>
            {
                try
                {
                    Parallel.ForEach(SVGGradientPalettes, gradient =>
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
            if (!paletteUnit.IsMainColor())
                mainColor = mainColor.ChangeHSL(paletteUnit.H, paletteUnit.S, paletteUnit.L);

            CanvasSvgNamedElement frontGradientSecondStop = CanvasSVG.FindElementById($"{gradientName}{paletteUnit.Number}");
            frontGradientSecondStop.SetStringAttribute("stop-color", mainColor.ToString(ColorOutputFormats.Hex));
        }
    }
}

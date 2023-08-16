using FIP.Core.Models;
using Microsoft.Graphics.Canvas.Svg;

namespace FIP.Core.Services
{
    /// <summary>
    /// Represents a service used for repaint svg gradients
    /// </summary>
    public interface ISVGPainterService
    {
        CanvasSvgDocument CanvasSVG { get; set; }

        void Initialize(CanvasSvgDocument canvasSVG);

        void ApplyColorPalette(FIPColor color);
    }
}

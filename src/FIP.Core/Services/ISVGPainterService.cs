using FIP.Core.Models;
using Microsoft.Graphics.Canvas.Svg;
using System.Threading.Tasks;

namespace FIP.Core.Services
{
    /// <summary>
    /// Represents a service used for repaint svg gradients
    /// </summary>
    public interface ISVGPainterService
    {
        CanvasSvgDocument CanvasSVG { get; set; }

        void Initialize(CanvasSvgDocument canvasSVG);

        Task ApplyColorPaletteAsync(FIPColor color);
    }
}

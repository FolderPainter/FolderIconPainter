using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace FIP.Backend.Services
{
    /// <summary>
    /// Represents a service used for image conversion.
    /// </summary>
    public interface IImageService
    {
        Task<bool> SaveBitmapAsIconAsync(Bitmap inputBitmap, string outputPath, int size = 256, bool preserveAspectRatio = false);

        Task<bool> ConvertBitmapToIconAsync(Bitmap inputBitmap, Stream output, int size = 256, bool preserveAspectRatio = false);
    }
}
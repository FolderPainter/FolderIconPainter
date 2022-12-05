using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIP.App.Helpers
{
    public static class ImageHelper
    {
        public async static Task<bool> ConvertBitmapToIconAsync(Bitmap inputBitmap, Stream output, int size = 256, bool preserveAspectRatio = false)
        {
            return await Task.Run(async Task<bool> () =>
            {
                if (inputBitmap != null)
                {
                    int width, height;
                    if (preserveAspectRatio)
                    {
                        width = size;
                        height = inputBitmap.Height / inputBitmap.Width * size;
                    }
                    else
                    {
                        width = height = size;
                    }
                    Bitmap newBitmap = new Bitmap(inputBitmap, new Size(width, height));
                    if (newBitmap != null)
                    {
                        // save the resized png into a memory stream for future use
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            newBitmap.Save(memoryStream, ImageFormat.Png);

                            BinaryWriter iconWriter = new BinaryWriter(output);
                            if (output != null && iconWriter != null)
                            {
                                // 0-1 reserved, 0
                                iconWriter.Write((byte)0);
                                iconWriter.Write((byte)0);

                                // 2-3 image type, 1 = icon, 2 = cursor
                                iconWriter.Write((short)1);

                                // 4-5 number of images
                                iconWriter.Write((short)1);

                                // image entry 1
                                // 0 image width
                                iconWriter.Write((byte)width);
                                // 1 image height
                                iconWriter.Write((byte)height);

                                // 2 number of colors
                                iconWriter.Write((byte)0);

                                // 3 reserved
                                iconWriter.Write((byte)0);

                                // 4-5 color planes
                                iconWriter.Write((short)0);

                                // 6-7 bits per pixel
                                iconWriter.Write((short)32);

                                // 8-11 size of image data
                                iconWriter.Write((int)memoryStream.Length);

                                // 12-15 offset of image data
                                iconWriter.Write((int)(6 + 16));

                                // write image data
                                // png data must contain the whole png data file
                                iconWriter.Write(memoryStream.ToArray());

                                iconWriter.Flush();

                                return await Task.FromResult(true);
                            }
                        }
                    }
                    return await Task.FromResult(false);
                }
                return await Task.FromResult(false);
            });
        }

        /// <summary>
        /// Saves bitmap as *.icon.
        /// </summary>
        /// <param name="inputBitmap">The input bitmap</param>
        /// <param name="outputPath">The output path</param>
        /// <param name="size">The size (16x16 px by default)</param>
        /// <param name="preserveAspectRatio">Preserve the aspect ratio</param>
        /// <returns>Wether or not the icon was succesfully generated</returns>
        public async static Task<bool> SaveBitmapAsIconAsync(Bitmap inputBitmap, string outputPath, int size = 256, bool preserveAspectRatio = false)
        {
            using (FileStream outputStream = new FileStream(outputPath, FileMode.OpenOrCreate))
            {
                return await ConvertBitmapToIconAsync(inputBitmap, outputStream, size, preserveAspectRatio);
            }
        }
    }
}

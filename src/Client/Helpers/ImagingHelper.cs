using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Client.Helpers
{
    /// <summary>
    /// Provides helper methods for imaging
    /// </summary>
    public static class ImagingHelper
    {
        /// <summary>
        /// Converts a PNG image to a icon (ico)
        /// </summary>
        /// <param name="input">The input stream</param>
        /// <param name="output">The output stream</param>
        /// <param name="size">The size (16x16 px by default)</param>
        /// <param name="preserveAspectRatio">Preserve the aspect ratio</param>
        /// <returns>Wether or not the icon was succesfully generated</returns>
        public static bool ConvertToIcon(Stream input, Stream output, int size = 256, bool preserveAspectRatio = false)
        {
            Bitmap inputBitmap = (Bitmap)Bitmap.FromStream(input);
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

                            return true;
                        }
                    }
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Converts a PNG image to a icon (ico)
        /// </summary>
        /// <param name="inputPath">The input path</param>
        /// <param name="outputPath">The output path</param>
        /// <param name="size">The size (16x16 px by default)</param>
        /// <param name="preserveAspectRatio">Preserve the aspect ratio</param>
        /// <returns>Wether or not the icon was succesfully generated</returns>
        public static bool ConvertToIcon(string inputPath, string outputPath, int size = 256, bool preserveAspectRatio = false)
        {
            using (FileStream inputStream = new FileStream(inputPath, FileMode.Open))
            using (FileStream outputStream = new FileStream(outputPath, FileMode.OpenOrCreate))
            {
                return ConvertToIcon(inputStream, outputStream, size, preserveAspectRatio);
            }
        }

        public static bool ConvertToIcon(Stream inputStream, string outputPath, int size = 256, bool preserveAspectRatio = false)
        {
            using (Stream stream = inputStream)
            using (FileStream outputStream = new FileStream(outputPath, FileMode.OpenOrCreate))
            {
                return ConvertToIcon(stream, outputStream, size, preserveAspectRatio);
            }
        }

        public static Bitmap Base64StringToBitmap(string base64String)
        {
            Bitmap bmpReturn;

            byte[] byteBuffer = Convert.FromBase64String(base64String);
            using (MemoryStream memoryStream = new MemoryStream(byteBuffer))
            {
                memoryStream.Position = 0;
                bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);
                memoryStream.Close();
                byteBuffer = null;
                return bmpReturn;
            }
        }

        public static Bitmap MakeImage(Size ImgSize, Bitmap foreImg, Bitmap backImg, byte s = 0)
        {
            Bitmap fimg = new Bitmap(foreImg, ImgSize);
            Bitmap bimg = new Bitmap(backImg, ImgSize);
            Bitmap bmp = new Bitmap(ImgSize.Width, ImgSize.Height);
            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color fm = fimg.GetPixel(i, j);
                    Color bm = bimg.GetPixel(i, j);
                    byte af = (byte)(fm.A * s / byte.MaxValue);
                    byte a = bm.A;
                    byte r = (byte)((fm.R * af + bm.R * (byte.MaxValue - af)) / byte.MaxValue);
                    byte g = (byte)((fm.G * af + bm.G * (byte.MaxValue - af)) / byte.MaxValue);
                    byte b = (byte)((fm.B * af + bm.B * (byte.MaxValue - af)) / byte.MaxValue);
                    bmp.SetPixel(i, j, Color.FromArgb(a, r, g, b));
                }
            return bmp;
        }
    }
}

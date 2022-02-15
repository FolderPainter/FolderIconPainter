using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor.Utilities;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Pages
{
    public partial class AddCustom : LayoutComponentBase
    {
        public MudColor pickerColor = "#689d94";
        public MudColor baseColor = "#b19f7f";
        public MudColor testCyan = "#689d94";
        public MudColor testRed = "#ff0000";
        [Inject] private IJSRuntime JSRuntime { get; set; }

        private IJSObjectReference module;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./filterGenerator.js");

                Filter = await module.InvokeAsync<string>("GenerateFilter", pickerColor.R, pickerColor.G, pickerColor.B);
                StateHasChanged();
            }
        }

        public async void ChangeColor(MudColor value)
        {
            pickerColor = value;

            Filter = await module.InvokeAsync<string>("GenerateFilter", pickerColor.R, pickerColor.G, pickerColor.B);
        }

        private async Task<string> GetBase64String(string domId, string filter)
        {
            return await module.InvokeAsync<string>("getBase64Image", domId, filter);
        }

        public async void SaveImageAsync()
        {
            var firstImgString = await GetBase64String("folderEmpty", Filter);
            var secondImgString = await GetBase64String("folderDoc", "none");
            //await JSRuntime.InvokeAsync<string>("console.log", );

            var firstImgBitmap = Base64StringToBitmap(firstImgString);
            var secondImgBitmap = Base64StringToBitmap(secondImgString);

            var newFolderImg = MakeImage(new Size(256, 256), secondImgBitmap, firstImgBitmap, 255);

          
            System.IO.MemoryStream ms = new MemoryStream();
            newFolderImg.Save(ms, ImageFormat.Png);
            byte[] byteImage = ms.ToArray();
            var SigBase64 = Convert.ToBase64String(byteImage); // Get Base64

            await JSRuntime.InvokeAsync<string>("console.log", SigBase64);

        }

        public string Filter { get; set; }

        private Bitmap Base64StringToBitmap(string base64String)
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

        private Bitmap MakeImage(Size ImgSize, Bitmap foreImg, Bitmap backImg, byte s = 0)
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

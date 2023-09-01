using FIP.App.Constants;
using FIP.App.Helpers;
using FIP.Core.Models;
using FIP.Core.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Bitmap = System.Drawing.Bitmap;

namespace FIP.App.Services
{
    public class FolderIconService : IFolderIconService
    {
        public FolderIconService()
        {
            Initialize(Path.Combine(ApplicationData.Current.LocalFolder.Path, 
                AppConstants.StorageSettings.IconsFolderName));
        }

        private string _folderPath;

        public void Initialize(string folderPath)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(folderPath));
                _folderPath = folderPath;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CreateFolderIconAsync(CustomIcon customIcon, Bitmap bitmap)
        {
            ArgumentNullException.ThrowIfNull(customIcon);
            ArgumentNullException.ThrowIfNull(bitmap);

            try
            {
                string newIconFolderPath = Path.Combine(_folderPath, customIcon.CategoryId.ToString());
                Directory.CreateDirectory(newIconFolderPath);
                StorageFolder iconsFolder = await StorageFolder.GetFolderFromPathAsync(newIconFolderPath);
                StorageFile newIconFile = await iconsFolder.CreateFileAsync($"{customIcon.Id}.ico");

                return await ImageHelper.SaveBitmapAsIconAsync(bitmap, newIconFile.Path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteFolderIconAsync(CustomIcon customIcon)
        {
            ArgumentNullException.ThrowIfNull(customIcon);

            try
            {
                string iconFolderPath = Path.Combine(_folderPath, customIcon.CategoryId.ToString());
             
                if (File.Exists(Path.Combine(iconFolderPath, $"{customIcon.Id}.ico")))
                {
                    StorageFolder iconsFolder = await StorageFolder.GetFolderFromPathAsync(iconFolderPath);
                    StorageFile iconFile = await iconsFolder.GetFileAsync($"{customIcon.Id}.ico");
                    await iconFile.DeleteAsync();
                    return !File.Exists(iconFile.Path);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool FolderIconExists(CustomIcon customIcon)
        {
            ArgumentNullException.ThrowIfNull(customIcon);

            try
            {
                string iconFolderPath = Path.Combine(_folderPath, customIcon.CategoryId.ToString());
                return File.Exists(Path.Combine(iconFolderPath, $"{customIcon.Id}.ico"));
            }
            catch (Exception)
            {
                throw;
            }
       
        }
    }
}

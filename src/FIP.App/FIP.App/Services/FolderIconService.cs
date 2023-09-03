using CommunityToolkit.WinUI.Helpers;
using FIP.App.Constants;
using FIP.App.Helpers;
using FIP.Core.Models;
using FIP.Core.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            ArgumentException.ThrowIfNullOrEmpty(folderPath);

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
                string iconPath = GetFolderIconPath(customIcon);

                if (File.Exists(iconPath))
                {
                    StorageFolder iconsFolder = await StorageFolder.GetFolderFromPathAsync(Path.GetDirectoryName(iconPath));
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

        public async Task<bool> MoveFolderIconAsync(CustomIcon customIcon, Category category)
        {
            ArgumentNullException.ThrowIfNull(customIcon);
            ArgumentNullException.ThrowIfNull(category);

            try
            {
                string iconPath = GetFolderIconPath(customIcon);

                if (File.Exists(iconPath))
                {
                    string otherCategoryFolderPath = Path.Combine(_folderPath, category.Id.ToString());
                    Directory.CreateDirectory(otherCategoryFolderPath);
                    StorageFolder otherCategoryFolder = await StorageFolder.GetFolderFromPathAsync(otherCategoryFolderPath);
                    StorageFile iconFile = await StorageFile.GetFileFromPathAsync(iconPath);
                    await iconFile.MoveAsync(otherCategoryFolder);

                    return File.Exists(Path.Combine(otherCategoryFolderPath, $"{customIcon.Id}.ico"));
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> MoveFolderIconsAsync(IEnumerable<CustomIcon> customIcons, Category category)
        {
            ArgumentNullException.ThrowIfNull(customIcons);
            ArgumentNullException.ThrowIfNull(category);

            foreach (var customIcon in customIcons)
            {
                if (!await MoveFolderIconAsync(customIcon, category))
                    return false;
            }

            return true;
        }

        public string GetFolderIconPath(CustomIcon customIcon)
        {
            ArgumentNullException.ThrowIfNull(customIcon);

            string iconFolderPath = Path.Combine(_folderPath, customIcon.CategoryId.ToString());
            return Path.Combine(iconFolderPath, $"{customIcon.Id}.ico");
        }

        public bool FolderIconExists(CustomIcon customIcon)
        {
            ArgumentNullException.ThrowIfNull(customIcon);

            return File.Exists(GetFolderIconPath(customIcon));
        }

        public async Task<StorageFile> CreateSvgFolderIconAsync(CustomIcon customIcon, string svgString)
        {
            ArgumentNullException.ThrowIfNull(customIcon);
            ArgumentException.ThrowIfNullOrEmpty(svgString);

            try
            {
                string newIconFolderPath = Path.Combine(_folderPath, customIcon.CategoryId.ToString());
                Directory.CreateDirectory(newIconFolderPath);
                StorageFolder iconsFolder = await StorageFolder.GetFolderFromPathAsync(newIconFolderPath);
                StorageFile newIconFile = await iconsFolder.CreateFileAsync($"{customIcon.Id}.svg");
                return await iconsFolder.WriteTextToFileAsync(svgString, newIconFile.Name);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteSvgFolderIconAsync(CustomIcon customIcon)
        {
            ArgumentNullException.ThrowIfNull(customIcon);

            try
            {
                string iconPath = GetSvgFolderIconPath(customIcon);

                if (File.Exists(iconPath))
                {
                    StorageFolder iconsFolder = await StorageFolder.GetFolderFromPathAsync(Path.GetDirectoryName(iconPath));
                    StorageFile iconFile = await iconsFolder.GetFileAsync($"{customIcon.Id}.svg");
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

        public string GetSvgFolderIconPath(CustomIcon customIcon)
        {
            ArgumentNullException.ThrowIfNull(customIcon);

            string iconFolderPath = Path.Combine(_folderPath, customIcon.CategoryId.ToString());
            return Path.Combine(iconFolderPath, $"{customIcon.Id}.svg");
        }


        public bool SvgFolderIconExists(CustomIcon customIcon)
        {
            ArgumentNullException.ThrowIfNull(customIcon);

            return File.Exists(GetSvgFolderIconPath(customIcon));
        }

        public async Task<bool> ForceCreateAsync(CustomIcon customIcon, Bitmap bitmap, string svgString)
        {
            if (await DeleteAsync(customIcon))
            {
                bool rasterIconCreated = await CreateFolderIconAsync(customIcon, bitmap);
                bool svgIconCreated = (await CreateSvgFolderIconAsync(customIcon, svgString)) != null;

                return rasterIconCreated && svgIconCreated;
            }

            return false;
        }

        public async Task<bool> DeleteAsync(CustomIcon customIcon)
        {
            bool rasterIconDeleted = true;
            bool svgIconDeleted = true;

            if (FolderIconExists(customIcon))
            {
                rasterIconDeleted = await DeleteFolderIconAsync(customIcon);
            }

            if (SvgFolderIconExists(customIcon))
            {
                svgIconDeleted = await DeleteSvgFolderIconAsync(customIcon);
            }

            return rasterIconDeleted && svgIconDeleted;
        }
    }
}

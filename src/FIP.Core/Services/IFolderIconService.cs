using FIP.Core.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Windows.Storage;

namespace FIP.Core.Services
{
    public interface IFolderIconService
    {
        public void Initialize(string filePath);

        public Task<bool> ForceCreateAsync(CustomIcon customIcon, Bitmap bitmap, string svgString);

        public Task<bool> DeleteAsync(CustomIcon customIcon);

        public Task<bool> CreateFolderIconAsync(CustomIcon customIcon, Bitmap bitmap);
               
        public Task<bool> DeleteFolderIconAsync(CustomIcon customIcon);

        public Task<bool> MoveFolderIconAsync(CustomIcon customIcon, Category category);

        public Task<bool> MoveFolderIconsAsync(IEnumerable<CustomIcon> customIcons, Category category);

        public string GetFolderIconPath(CustomIcon customIcon);

        public bool FolderIconExists(CustomIcon customIcon);

        public Task<StorageFile> CreateSvgFolderIconAsync(CustomIcon customIcon, string svgString);

        public Task<bool> DeleteSvgFolderIconAsync(CustomIcon customIcon);

        public string GetSvgFolderIconPath(CustomIcon customIcon);

        public bool SvgFolderIconExists(CustomIcon customIcon);
    }
}

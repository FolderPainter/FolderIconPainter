using FIP.Core.Models;
using System.Drawing;
using System.Threading.Tasks;

namespace FIP.Core.Services
{
    public interface IFolderIconService
    {
        public void Initialize(string filePath);

        public Task<bool> CreateFolderIconAsync(CustomIcon customIcon, Bitmap bitmap);

        public Task<bool> DeleteFolderIconAsync(CustomIcon customIcon);
    }
}

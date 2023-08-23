using FIP.App.Constants;
using FIP.Core.Services;
using FIP.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;

namespace FIP.App.Services
{
    public class CustomIconStorageService : BaseJSONStorageService, ICustomIconStorageService
    {
        private IEnumerable<CustomIconViewModel> _customIcons;

        public event EventHandler OnCustomIconsUpdated;

        public CustomIconStorageService()
        {
            Initialize(Path.Combine(ApplicationData.Current.LocalFolder.Path,
                AppConstants.StorageSettings.StorageFolderName, AppConstants.StorageSettings.FolderIconsStorageFileName));

            _customIcons = GetAllValues<CustomIconViewModel>();
        }

        public IEnumerable<CustomIconViewModel> CustomIcons
        {
            get => _customIcons;
            set
            {
                _customIcons = value;
                SetAllValues(value);
                OnCustomIconsUpdated.Invoke(this, EventArgs.Empty);
            }
        }

        public CustomIconViewModel GetCustomIconById(Guid id)
        {
            return CustomIcons.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<CustomIconViewModel> GetCustomIconsByCategoryId(Guid categoryId)
        {
            return CustomIcons.Where(x => x.CategoryId == categoryId);
        }

        public void AddCustomIcon(CustomIconViewModel customIcon)
        {
            var customIcons = CustomIcons.ToList();
            customIcons.Add(customIcon);
            CustomIcons = customIcons;
        }

        public void DeleteCustomIconById(Guid id)
        {
            var customIcons = CustomIcons.ToList();
            customIcons.RemoveAll(category => category.Id == id);
            CustomIcons = customIcons;
        }

        public void DeleteCustomIconByCategoryId(Guid categoryId)
        {
            var customIcons = CustomIcons.ToList();
            customIcons.RemoveAll(customIcon => customIcon.CategoryId == categoryId);
            CustomIcons = customIcons;
        }
    }
}

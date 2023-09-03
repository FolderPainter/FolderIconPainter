using FIP.App.Constants;
using FIP.Core.Models;
using FIP.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;

namespace FIP.App.Services
{
    public class CustomIconStorageService : BaseJSONStorageService, ICustomIconStorageService
    {
        private IEnumerable<CustomIcon> _customIcons;

        public event EventHandler OnCustomIconsUpdated;

        public CustomIconStorageService()
        {
            Initialize(Path.Combine(ApplicationData.Current.LocalFolder.Path,
                AppConstants.StorageSettings.StorageFolderName, AppConstants.StorageSettings.FolderIconsStorageFileName));

            _customIcons = GetAllValues<CustomIcon>();
        }

        public IEnumerable<CustomIcon> CustomIcons
        {
            get => _customIcons;
            set
            {
                _customIcons = value;
                SetAllValues(value);
                OnCustomIconsUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public CustomIcon GetCustomIconById(Guid id)
        {
            return CustomIcons.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<CustomIcon> GetCustomIconsByCategoryId(Guid categoryId)
        {
            return CustomIcons.Where(x => x.CategoryId == categoryId).Select(item => item.ShallowCopy());
        }

        public void AddCustomIcon(CustomIcon customIcon)
        {
            ArgumentNullException.ThrowIfNull(customIcon);

            var customIcons = CustomIcons.ToList();
            customIcons.Add(customIcon);
            CustomIcons = customIcons;
        }

        public void UpdateCustomIcon(CustomIcon customIcon)
        {
            ArgumentNullException.ThrowIfNull(customIcon);

            var customIcons = CustomIcons.ToList();
            customIcons.RemoveAll(ci => ci.Id == customIcon.Id);
            customIcons.Add(customIcon);
            CustomIcons = customIcons;
        }

        public void PostCustomIcon(CustomIcon customIcon)
        {
            ArgumentNullException.ThrowIfNull(customIcon);

            if (CustomIcons.Any(ci => ci.Id == customIcon.Id))
            {
                UpdateCustomIcon(customIcon);
            }
            else
            {
                AddCustomIcon(customIcon);
            }
        }

        public void DeleteCustomIconById(Guid id)
        {
            var customIcons = CustomIcons.ToList();
            customIcons.RemoveAll(customIcon => customIcon.Id == id);
            CustomIcons = customIcons;
        }

        public void DeleteCustomIconsByCategoryId(Guid categoryId)
        {
            var customIcons = CustomIcons.ToList();
            customIcons.RemoveAll(customIcon => customIcon.CategoryId == categoryId);
            CustomIcons = customIcons;
        }

        public void MoveCustomIconsToOtherCategory(Guid categoryId, Guid otherCategoryId)
        {
            var editedCustomIcons = GetCustomIconsByCategoryId(categoryId).ToList();

            var newCustomIcons = CustomIcons.ToList();
            newCustomIcons.RemoveAll(customIcon => customIcon.CategoryId == categoryId);

            newCustomIcons.AddRange(editedCustomIcons.Select(ci =>
            {
                ci.CategoryId = otherCategoryId;
                return ci;
            }));

            CustomIcons = newCustomIcons;
        }

        public void MoveCustomIconsToOtherCategory(IEnumerable<CustomIcon> customIcons, Guid otherCategoryId)
        {
            var editedCustomIcons = customIcons.Select(ci =>
            {
                ci.CategoryId = otherCategoryId;
                return ci;
            });

            var newCustomIcons = CustomIcons.ToList();

            foreach (var customIcon in customIcons)
            {
                newCustomIcons.Remove(customIcon);
            }

            newCustomIcons.AddRange(editedCustomIcons);
            CustomIcons = newCustomIcons;
        }
    }
}

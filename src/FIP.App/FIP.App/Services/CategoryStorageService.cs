using FIP.App.Constants;
using FIP.Core.ViewModels;
using FIP.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;

namespace FIP.App.Services
{
    class CategoryStorageService : BaseJSONStorageService, ICategoryStorageService
    {
        private IEnumerable<CategoryViewModel> _categories;

        public event EventHandler OnCategoriesUpdated;

        public CategoryStorageService()
        {
            Initialize(Path.Combine(ApplicationData.Current.LocalFolder.Path,
                AppConstants.StorageSettings.StorageFolderName, AppConstants.StorageSettings.CategoriesStorageFileName));

            _categories = GetAllValues<CategoryViewModel>();
        }

        public IEnumerable<CategoryViewModel> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                SetAllValues(value);
                OnCategoriesUpdated.Invoke(this, EventArgs.Empty);
            }
        }

        public CategoryViewModel GetCategoryById(Guid id)
        {
            return Categories.SingleOrDefault(x => x.Id == id);
        }

        public void AddCategory(CategoryViewModel category)
        {
            var categories = Categories.ToList();
            categories.Add(category);
            Categories = categories;
        }

        public void DeleteCategoryById(Guid id)
        {
            var categories = Categories.ToList();
            categories.RemoveAll(category => category.Id == id);
            Categories = categories;
        }
    }
}

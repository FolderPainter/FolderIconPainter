using FIP.Core.ViewModels;
using System;
using System.Collections.Generic;

namespace FIP.Core.Services
{
    public interface ICategoryStorageService
    {
        event EventHandler OnCategoriesUpdated;

        IEnumerable<CategoryViewModel> Categories { get; set; }

        CategoryViewModel GetCategoryById(Guid id);

        void AddCategory(CategoryViewModel category);

        void DeleteCategoryById(Guid id);
    }
}

﻿using FIP.Core.Models;
using System;
using System.Collections.Generic;

namespace FIP.Core.Services
{
    public interface ICategoryStorageService
    {
        event EventHandler OnCategoriesUpdated;

        IEnumerable<Category> Categories { get; set; }

        Category GetCategoryById(Guid id);

        Category AddCategory(Category category);

        Category UpdateCategory(Category category);

        Category PostCategory(Category category);

        void DeleteCategoryById(Guid id);
    }
}

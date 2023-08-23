using FIP.Core.ViewModels;
using System;
using System.Collections.Generic;

namespace FIP.Core.Services
{
    public interface ICustomIconStorageService
    {
        event EventHandler OnCustomIconsUpdated;

        IEnumerable<CustomIconViewModel> CustomIcons { get; set; }

        CustomIconViewModel GetCustomIconById(Guid id);

        IEnumerable<CustomIconViewModel> GetCustomIconsByCategoryId(Guid categoryId);

        void AddCustomIcon(CustomIconViewModel category);

        void DeleteCustomIconById(Guid id);

        void DeleteCustomIconByCategoryId(Guid categoryId);
    }
}

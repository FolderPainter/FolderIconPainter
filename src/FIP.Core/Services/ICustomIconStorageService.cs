using FIP.Core.Models;
using System;
using System.Collections.Generic;

namespace FIP.Core.Services
{
    public interface ICustomIconStorageService
    {
        event EventHandler OnCustomIconsUpdated;

        IEnumerable<CustomIcon> CustomIcons { get; set; }

        CustomIcon GetCustomIconById(Guid id);

        IEnumerable<CustomIcon> GetCustomIconsByCategoryId(Guid categoryId);

        void AddCustomIcon(CustomIcon category);

        void DeleteCustomIconById(Guid id);

        void DeleteCustomIconByCategoryId(Guid categoryId);
    }
}

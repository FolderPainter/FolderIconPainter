using CommunityToolkit.Mvvm.DependencyInjection;
using FIP.App.Constants;
using FIP.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FIP.App.ViewModels
{
    public class AllFolderIconsViewModel
    {
        private ICategoryStorageService CategoryStorageService { get; } = Ioc.Default.GetRequiredService<ICategoryStorageService>();

        private ICustomIconStorageService CustomIconStorageService { get; } = Ioc.Default.GetRequiredService<ICustomIconStorageService>();

        private GroupInfoList GetDefaultIcons()
        {
            var fipHexColors = DefaultColors.GetAllColors().ToList();
            var dropZoneViewModels = new List<DropZoneViewModel>();

            //fipHexColors.ForEach(c => dropZoneViewModels.Add(new DropZoneViewModel(c)));

            return new GroupInfoList(dropZoneViewModels)
            {
                Key = Guid.Empty,
                Title = "Default Icons"
            };
        }

        public List<GroupInfoList> GetCustomIcons()
        {
            var icons = CustomIconStorageService.CustomIcons;

            var query = from item in icons
                        group item by item.CategoryId into g
                        select new GroupInfoList(g.Select(g => new DropZoneViewModel(new (g))))
                        {
                            Key = g.Key,
                            Title = g.Key == Guid.Empty ? "No Category" : CategoryStorageService.GetCategoryById(g.Key).Name
                        };

            //return new ObservableCollection<GroupInfoList>(query.Prepend(GetDefaultIcons()));
            return new List<GroupInfoList>(query);
        }
    }

    public class GroupInfoList : List<DropZoneViewModel>
    {
        public GroupInfoList(IEnumerable<DropZoneViewModel> items) : base(items) { }

        public object Key { get; set; }

        public string Title { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}

using CommunityToolkit.Mvvm.DependencyInjection;
using FIP.App.Constants;
using FIP.Core.Models;
using FIP.Core.Services;
using FIP.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace FIP.App.ViewModels
{
    public class AllFolderIconsViewModel
    {
        private ICategoryStorageService CategoryStorageService { get; } = Ioc.Default.GetRequiredService<ICategoryStorageService>();

        private ICustomIconStorageService CustomIconStorageService { get; } = Ioc.Default.GetRequiredService<ICustomIconStorageService>();

        public IEnumerable<CategoryViewModel> Categories { get => CategoryStorageService.Categories.Select(c => new CategoryViewModel(c)).Append(new (new Category { Id = Guid.Empty })); }

        private async Task<GroupInfoList> GetDefaultIconsAsync()
        {
            Uri sourceUri = new Uri(AppConstants.AssetPaths.DefaultIconsJSON);
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(sourceUri);
            string jsonText = await FileIO.ReadTextAsync(file);

            var defaultCustomIcons = JsonSerializer.Deserialize<List<CustomIcon>>(jsonText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var dropZoneViewModels = new List<DropZoneViewModel>();

            defaultCustomIcons.ForEach(c => dropZoneViewModels.Add(
                new DropZoneViewModel(new CustomIconViewModel(c))));

            return new GroupInfoList(dropZoneViewModels)
            {
                Key = AppConstants.DefaultCategoryId,
                Title = "Default Icons"
            };
        }

        public async Task<List<GroupInfoList>> GetCustomIconsAsync()
        {
            var icons = CustomIconStorageService.CustomIcons;

            var query = from item in icons
                        group item by item.CategoryId into g
                        select new GroupInfoList(g.Select(g => new DropZoneViewModel(new (g))))
                        {
                            Key = g.Key,
                            Title = g.Key == Guid.Empty ? "No Category" : CategoryStorageService.GetCategoryById(g.Key).Name
                        };
            query = query.Prepend(await GetDefaultIconsAsync());

            //return new ObservableCollection<GroupInfoList>(query.Prepend(GetDefaultIcons()));
            return new List<GroupInfoList>(query);
        }
    }

    public class GroupInfoList : List<DropZoneViewModel>
    {
        public GroupInfoList(IEnumerable<DropZoneViewModel> items) : base(items) { }

        public Guid Key { get; set; }

        public string Title { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}

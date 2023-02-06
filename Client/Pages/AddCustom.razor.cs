using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Utilities;
using System;
using System.IO;
using System.Threading.Tasks;
using Client.Helpers;
using Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Application.Features.Categories.Queries;
using System.Net.Http;
using Newtonsoft.Json;
using Client.Infrastructure.Services;
using Application.Features.Categories.Commands;
using Application.Features.CustomFolders.Commands;

namespace Client.Pages
{
    public partial class AddCustom : LayoutComponentBase
    {
        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Inject] private CategoryService CategoryService { get; set; }

        [Inject] private CustomFolderService CustomFolderService { get; set; }

        MudColor pickerColor = "#3cec53";
        IJSObjectReference module;
        bool success;
        MudForm form;

        string Filter { get; set; }

        public IEnumerable<CategoryDto> Categories { get; set; }

        public CategoryDto SelectedCategory = new CategoryDto { Id = 0 };

        [Parameter] public string IconName { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await form.Validate();
            if (firstRender)
            {
                Categories = await CategoryService.SearchAsync(new SearchCategoriesRequest { SortDirection = Domain.Enums.SortDirection.Descending });
                module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./filterGenerator.js");

                Filter = await module.InvokeAsync<string>("GenerateFilter", pickerColor.R, pickerColor.G, pickerColor.B);

                StateHasChanged();
            }
        }

        public async void ChangeColor(MudColor value)
        {
            pickerColor = value;
            Filter = await module.InvokeAsync<string>("GenerateFilter", pickerColor.R, pickerColor.G, pickerColor.B);
        }

        public async void SaveImageAsync()
        {
            await form.Validate();
            if (!success)
                return;
            try
            {
                if (!Categories.Any(c => c.Name.Equals(SelectedCategory.Name)))
                {
                    SelectedCategory.Id = await CategoryService.CreateAsync(new CreateCategoryRequest { Name = SelectedCategory.Name });
                }

                if (SelectedCategory.Id != 0)
                {
                    var folderIconStrings = await module.InvokeAsync<FolderIconStrings>("overlayImages", "folderEmpty", "folderDoc", Filter);

                    if (!folderIconStrings.IsNullOrWhiteSpace())
                    {

                        byte[] byteBuffer = Convert.FromBase64String(folderIconStrings.EmptyFolderIcon);
                        using (MemoryStream ms = new MemoryStream(byteBuffer))
                        {
                            ImagingHelper.ConvertToIcon(ms, $"wwwroot/icons/custom/empty/{IconName}_{SelectedCategory.Id}.ico");
                        }

                        byteBuffer = Convert.FromBase64String(folderIconStrings.DefFolderIcon);
                        using (MemoryStream ms = new MemoryStream(byteBuffer))
                        {
                            ImagingHelper.ConvertToIcon(ms, $"wwwroot/icons/custom/def/{IconName}_{SelectedCategory.Id}.ico");
                        }

                        var res = await CustomFolderService.CreateAsync(new CreateCustomFolderRequest { 
                            Name = IconName, 
                            CategoryId = SelectedCategory.Id,
                            ColorHex = pickerColor.ToString(MudColorOutputFormats.Hex)
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("console.log", ex.Message);
            }
        }

        public async void CreateCategoryAsync()
        {
        }

        public async Task OnCategoryTextChanged(string value)
        {
            await form.Validate();
        }

        public async Task<IEnumerable<string>> SearchCategoryAsync(string value)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return Categories.Select(c => c.Name).ToList();
                }

                var res = Categories.Select(c => c.Name).Where(c => c.Contains(value, StringComparison.InvariantCultureIgnoreCase));
                return res.ToList();
            });
        }


        public async Task<IEnumerable<CategoryDto>> SearchCategoryAsync2(string value)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return Categories;
                }

                return Categories.Where(c => c.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
            });
        }
    }
}

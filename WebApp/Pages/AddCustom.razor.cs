using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Utilities;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApp.Helpers;
using WebApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Application.Features.Categories.Queries.GetAll;

namespace WebApp.Pages
{
    public partial class AddCustom : LayoutComponentBase
    {
        [Inject] private IJSRuntime JSRuntime { get; set; }

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
                module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./filterGenerator.js");
                //Categories = await CategoryService.GetAllCategories(CancellationToken.None);
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
                var folderIconStrings = await module.InvokeAsync<FolderIconStrings>("overlayImages", "folderEmpty", "folderDoc", Filter);

                if (!folderIconStrings.IsNullOrWhiteSpace())
                {
                    byte[] byteBuffer = Convert.FromBase64String(folderIconStrings.EmptyFolderIcon);
                    using (MemoryStream ms = new MemoryStream(byteBuffer))
                    {
                        ImagingHelper.ConvertToIcon(ms, $"wwwroot/icons/custom/empty/{IconName}.ico");
                    }

                    byteBuffer = Convert.FromBase64String(folderIconStrings.DefFolderIcon);
                    using (MemoryStream ms = new MemoryStream(byteBuffer))
                    {
                        ImagingHelper.ConvertToIcon(ms, $"wwwroot/icons/custom/def/{IconName}.ico");
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
    }
}

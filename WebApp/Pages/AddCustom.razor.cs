using Application.Interfaces.Services;
using Infrastructure.Services;
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
using Application.Features.Category.Queries.GetAll;

namespace WebApp.Pages
{
    public partial class AddCustom : LayoutComponentBase
    {
        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Inject] private ICategoryService CategoryService { get; set; }

        MudColor pickerColor = "#3cec53";
        IJSObjectReference module;
        bool success;
        bool categoryFinded;
        MudForm form;

        string Filter { get; set; }

        public IEnumerable<GetAllCategoriesResponse> Categories { get; set; }

        [Parameter] public string IconName { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./filterGenerator.js");
                Categories = await CategoryService.GetAllCategories(CancellationToken.None);

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
            if (!String.IsNullOrEmpty(value))
                categoryFinded = Categories.Any(c => c.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
           

            await form.Validate();
        }


        public async Task<IEnumerable<GetAllCategoriesResponse>> SearchCategoryAsync(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Categories;
            }

            var res = Categories.Where(c => c.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
            categoryFinded = res.Count() > 0;
            return res;
        }
    }
}

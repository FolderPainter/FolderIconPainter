using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor.Utilities;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Pages
{
    public partial class AddCustom : LayoutComponentBase
    {
        public MudColor pickerColor = "#689d94";
        public MudColor baseColor = "#b19f7f";
        public MudColor testCyan = "#689d94";
        public MudColor testRed = "#ff0000";

        [Inject] private IJSRuntime JSRuntime { get; set; }

        private IJSObjectReference module;

        public string Filter { get; set; }

        [Parameter] public string IconName { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
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
    }
}

using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Shared.Components
{
    public partial class DragDropZone : LayoutComponentBase
    {
        private string color = "black";
        private string _dragEnterStyle;

        private IJSRuntime JSRuntime;

        private IJSObjectReference module;
        private IJSObjectReference dropZoneInstance;

        private ElementReference dropZoneElement;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./dropZone.js");

                dropZoneInstance = await module.InvokeAsync<IJSObjectReference>("initializeFileDropZone", dropZoneElement);
                StateHasChanged();
            }
        }

        public async void OnDrop(DragEventArgs evt)
        {
            var f = await module.InvokeAsync<string[]>("GetFiles");
            await JSRuntime.InvokeAsync<string>("console.log", f);

            StateHasChanged();
        }

        private async void OnClicked()
        {
            var mainWindow = Electron.WindowManager.BrowserWindows.First();
            var options = new OpenDialogOptions
            {
                Properties = new OpenDialogProperty[] {
                    OpenDialogProperty.openDirectory
                }
            };

            string[] files = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, options);
            await JSRuntime.InvokeAsync<string>("console.log", files);

            Electron.IpcMain.Send(mainWindow, "select-directory-reply", files);
        }

        [Parameter] public string Color
        {
            get => color;
            set => color = value;
        }


        // Unregister the drop zone events
        public async ValueTask DisposeAsync()
        {
            if (dropZoneInstance != null)
            {
                await dropZoneInstance.InvokeVoidAsync("dispose");
                await dropZoneInstance.DisposeAsync();
            }

            if (module != null)
            {
                await module.DisposeAsync();
            }
        }
    }
}

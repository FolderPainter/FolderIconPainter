using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Services;

namespace WebApp.Shared.Components
{
    public partial class DragDropZone : LayoutComponentBase
    {
        private string color = "black";
        private int index = 0;
        private string _dragEnterStyle;

        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private IconService IconService { get; set; }

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

        private bool IsDirectoryEmpty(string path)
        {
            var filesCount = Directory.EnumerateFileSystemEntries(path).Count();
            if (File.Exists(path + @"\desktop.ini"))
                return filesCount - 3 == 0;
            return filesCount == 0;
        }

        private async void SetIcons(string[] folders)
        {
            foreach (var path in folders)
            {
                string icoPath = Path.Combine(Directory.GetCurrentDirectory() +
                $"\\wwwroot\\icons\\{(IsDirectoryEmpty(path) ? "empty" : "def")}\\{index}.ico");

                var res = IconService.SettingIcons(path, icoPath);
            }
        }

        public async void OnDrop(DragEventArgs evt)
        {
            _dragEnterStyle = null;

            string[] folders = await module.InvokeAsync<string[]>("GetFiles");
            SetIcons(folders);
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

            string[] folders = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, options);
            SetIcons(folders);
            //Electron.IpcMain.Send(mainWindow, "select-directory-reply", folders);
        }

        [Parameter] public string Color
        {
            get => color;
            set => color = value;
        }

        [Parameter] public int Index
        {
            get => index;
            set => index = value;
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

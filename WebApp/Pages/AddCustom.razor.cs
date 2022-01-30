using ElectronNET.API;
using ElectronNET.API.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Pages
{
    public partial class AddCustom
    {
        private async void OnButtonClicked()
        {
            var mainWindow = Electron.WindowManager.BrowserWindows.First();
            var options = new OpenDialogOptions
            {
                Properties = new OpenDialogProperty[] {
                OpenDialogProperty.openDirectory,
                OpenDialogProperty.openFile
                }
            };

            string[] files = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, options);
            Electron.IpcMain.Send(mainWindow, "select-directory-reply", files);
        }
    }
}

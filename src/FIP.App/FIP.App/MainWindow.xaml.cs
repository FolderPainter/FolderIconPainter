using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FIP.App
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx
    {
        public MainWindow()
        {
            InitializeComponent();

            PersistenceId = "FilesMainWindow";

            EnsureEarlyWindow();
        }

        private void EnsureEarlyWindow()
        {
            // Set title
            AppWindow.Title = "FolderIconPainter";

            // Set icon
            //AppWindow.SetIcon(Path.Combine(Package.Current.InstalledLocation.Path, Constants.AssetPaths.Logo));

            // Extend title bar
            AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;

            // Set window buttons background to transparent
            AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            AppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            // Set min size
            base.MinHeight = 328;
            base.MinWidth = 516;
        }

    }
}

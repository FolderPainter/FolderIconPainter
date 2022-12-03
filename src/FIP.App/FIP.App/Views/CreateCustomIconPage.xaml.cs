// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.Graphics.Canvas.Svg;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using System.Threading.Tasks;
using Windows.Storage.Streams;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FIP.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateCustomIconPage : Page
    {
        public CreateCustomIconPage()
        {
            this.InitializeComponent();
            mainColorPicker.Color = Color.FromArgb(100, 22, 22, 22);
        }


        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            Uri localUri = new Uri("ms-appx:///Assets/win11-folder-default.svg");

            CanvasSvgDocument svgDocument = null;

            GetImage().Wait();
            async Task GetImage()
            {
                await Task.Run(async () =>
                {
                    var file = await StorageFile.GetFileFromApplicationUriAsync(localUri);
                    IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);

                    svgDocument = new CanvasSvgDocument(sender);
                    CanvasSvgNamedElement element = await svgDocument.LoadElementAsync(stream);

                    CanvasSvgNamedElement gChild = element.FirstChild as CanvasSvgNamedElement;
                    gChild.SetStringAttribute("fill", "#DC143C");
                    svgDocument.Root.AppendChild(element);

                }).ConfigureAwait(false);
            }

            args.DrawingSession.DrawSvg(svgDocument, sender.Size);
        }
    }
}

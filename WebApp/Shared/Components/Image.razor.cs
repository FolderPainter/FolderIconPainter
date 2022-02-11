using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Extensions;
using MudBlazor.Utilities;
using WebApp.Enums;

namespace WebApp.Shared.Components
{
    public partial class Image : MudComponentBase
    {
        protected string Classname =>
            new CssBuilder("mud-image")
                .AddClass("fluid", Fluid)
                .AddClass($"object-{ObjectFit.ToDescriptionString()}")
                .AddClass($"object-{ObjectPosition.ToDescriptionString()}")
                .AddClass($"mud-elevation-{Elevation}", Elevation > 0)
                .AddClass(Class)
                .Build();

        /// <summary>
        /// Applies the fluid class so the image scales with the parent width.
        /// </summary>
        [Parameter] public bool Fluid { get; set; }

        /// <summary>
        /// Specifies the path to the image.
        /// </summary>
        [Parameter] public string Src { get; set; }

        /// <summary>
        /// Specifies an alternate text for the image.
        /// </summary>
        [Parameter] public string Alt { get; set; }

        /// <summary>
        /// Specifies the height of the image in px.
        /// </summary>
        [Parameter] public int? Height { get; set; }

        /// <summary>
        /// Specifies the width of the image in px.
        /// </summary>
        [Parameter] public int? Width { get; set; }

        /// <summary>
        /// The higher the number, the heavier the drop-shadow.
        /// </summary>
        [Parameter] public int Elevation { set; get; }

        /// <summary>
        /// Controls how the image should be resized.
        /// </summary>
        [Parameter] public ObjectFit ObjectFit { set; get; } = ObjectFit.Fill;

        /// <summary>
        /// Controls how the image should positioned within its container.
        /// </summary>
        [Parameter] public ObjectPosition ObjectPosition { set; get; } = ObjectPosition.Center;
    }

}

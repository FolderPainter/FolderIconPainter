using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace WebApp.Shared.Components
{
    public partial class DragDropZone : LayoutComponentBase
    {
        string color = "red";


        [Parameter] public string Color
        {
            get => color;
            set => color = value;
        }

        [Parameter] public string IconPath { get; set; }
    }
}

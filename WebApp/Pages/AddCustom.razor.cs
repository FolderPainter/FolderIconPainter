using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor.Utilities;
using System.Globalization;
using System.Threading.Tasks;
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

        //private ColorHSL NewColor = new();

        //private ColorHSL CyanTest = new();

        //private ColorHSL RedTest = new();

        //protected override void OnInitialized()
        //{
        //    NewColor.H = CyanTest.H = testCyan.H - baseColor.H;
        //    NewColor.S = CyanTest.S = 100d + ((baseColor.S - testCyan.S) * 100d);
        //    NewColor.L = CyanTest.L = 100d + ((testCyan.L - baseColor.L) * 100d);

        //    RedTest.H = testRed.H - baseColor.H;
        //    RedTest.S = 100d + ((baseColor.S - testRed.S) * 100d);
        //    RedTest.L = 100d + ((testRed.L - baseColor.L) * 100d);

        //    base.OnInitialized();
        //}

        public async void ChangeColor(MudColor value)
        {
            pickerColor = value;

            Filter = await module.InvokeAsync<string>("GenerateFilter", pickerColor.R, pickerColor.G, pickerColor.B);
        }

        public string Filter { get; set; }
    }
}

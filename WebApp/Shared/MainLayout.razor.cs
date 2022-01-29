using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebApp.Services;
using WebApp.Services.UserPreferences;

namespace WebApp.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject] private LayoutService LayoutService { get; set; }

        public bool IsDarkMode { get; private set; }

        protected override void OnInitialized()
        {
            LayoutService.MajorUpdateOccured += LayoutServiceOnMajorUpdateOccured;
            LayoutService.SetBaseTheme(Theme.DefaultTheme);
            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                await ApplyUserPreferences();
                StateHasChanged();
            }
        }

        private async Task ApplyUserPreferences()
        {
            //var defaultDarkMode = await _mudThemeProvider.GetSystemPreference();
            await LayoutService.ApplyUserPreferences(true);
        }

        public void Dispose()
        {
            LayoutService.MajorUpdateOccured -= LayoutServiceOnMajorUpdateOccured;
        }

        private void LayoutServiceOnMajorUpdateOccured(object sender, EventArgs e) => StateHasChanged();
    }
}

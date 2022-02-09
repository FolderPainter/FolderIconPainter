using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebApp.Services;
using WebApp.Services.UserPreferences;
using ElectronNET.API;
using ElectronNET.API.Entities;
using static MudBlazor.CategoryTypes;
using MudBlazor.Services;

namespace WebApp.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject] private LayoutService LayoutService { get; set; }

        [Inject] private IBreakpointService BreakpointListener { get; set; }

        public bool IsDarkMode { get; private set; }

        private Guid _subscriptionId;
        protected override void OnInitialized()
        {
            LayoutService.MajorUpdateOccured += LayoutServiceOnMajorUpdateOccured;
            LayoutService.SetBaseTheme(Theme.DefaultTheme);
            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            if (firstRender)
            {

                var subscriptionResult = await BreakpointListener.Subscribe((breakpoint) =>
                {
                    OnDrawerOpenChanged((breakpoint == Breakpoint.Md));
                    InvokeAsync(StateHasChanged);
                }, new MudBlazor.Services.ResizeOptions
                {
                    ReportRate = 250,
                    NotifyOnBreakpointOnly = true,
                });

                _subscriptionId = subscriptionResult.SubscriptionId;
                await ApplyUserPreferences();
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        public async ValueTask DisposeAsync() => await BreakpointListener.Unsubscribe(_subscriptionId);

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


        private async Task OpenGitHub()
        {
            await Electron.Shell.OpenExternalAsync("https://github.com/ColdForeign/FolderIconPainter/");
        }


        //private NavMenu _navMenuRef;
        private bool _drawerOpen = false;

        private void ToggleDrawer()
        {
            _drawerOpen = !_drawerOpen;
        }

        private void OnDrawerOpenChanged(bool value)
        {
            _drawerOpen = value;
            StateHasChanged();
        }
    }
}

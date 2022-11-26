using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor.Services;
using Microsoft.JSInterop;
using Client.Enums;
using Client.Infrastructure.Services;
using Client.Infrastructure.Settings;

namespace Client.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject] private LayoutService LayoutService { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] IResizeService ResizeService { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; }

        public bool IsDarkMode { get; private set; }

        protected override void OnInitialized()
        {
            LayoutService.MajorUpdateOccured += LayoutServiceOnMajorUpdateOccured;
            LayoutService.SetBaseTheme(Theme.DefaultTheme);
            base.OnInitialized();
        }

        private Guid _subscriptionId;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _subscriptionId = await ResizeService.Subscribe((size) =>
                {
                    if (size.Width > 960)
                    {
                        OnDrawerOpenChanged(false);
                    }

                    InvokeAsync(StateHasChanged);
                }, new ResizeOptions
                {
                    ReportRate = 50,
                    NotifyOnBreakpointOnly = false,
                });

                var size = await ResizeService.GetBrowserWindowSize();

                await ApplyUserPreferences();
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        public async ValueTask DisposeAsync() => await ResizeService.Unsubscribe(_subscriptionId);

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
            //await Electron.Shell.OpenExternalAsync("https://github.com/ColdForeign/FolderIconPainter/");
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

        private string GetActiveClass(BasePage page)
        {
            return page == GetDocsBasePage(NavigationManager.Uri) ? "mud-chip-text mud-chip-color-primary ml-3" : "ml-3";
        }
        public BasePage GetDocsBasePage(string uri)
        {
            if (uri.Contains("/addcustom"))
                return BasePage.AddCustom;
            else if (uri.Contains("/settings"))
                return BasePage.Settings;
            else
                return BasePage.Home;
        }
    }
}

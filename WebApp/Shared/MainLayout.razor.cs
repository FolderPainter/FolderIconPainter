using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebApp.Services;
using WebApp.Services.UserPreferences;

namespace WebApp.Shared
{
    public partial class MainLayout: LayoutComponentBase
    {
        public bool IsDarkMode { get; private set; }

        private MudTheme _currentTheme;

        protected override async Task OnInitializedAsync()
        {
            _currentTheme = Theme.DefaultTheme;
            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                _currentTheme = await _userPreferencesService.GetCurrentThemeAsync();
                StateHasChanged();
            }
        }

        private async Task DarkMode()
        {
            IsDarkMode = await _userPreferencesService.ToggleDarkModeAsync();
            _currentTheme = IsDarkMode
                ? Theme.DefaultTheme
                : Theme.DarkTheme;
        }
    }
}

using System;
using System.Threading.Tasks;
using MudBlazor;
using Client.Infrastructure.Preferences;

namespace Client.Infrastructure.Services
{
    public class LayoutService
    {
        private readonly IUserPreferencesManager _userPreferencesManager;

        private UserPreferences _userPreferences;

        public bool IsDarkMode { get; private set; } = false;

        public MudTheme CurrentTheme { get; private set; }

        public LayoutService(IUserPreferencesManager userPreferencesManager)
        {
            _userPreferencesManager = userPreferencesManager;
        }

        public void SetDarkMode(bool value)
        {
            IsDarkMode = value;
        }

        public async Task ApplyUserPreferences(bool isDarkModeDefaultTheme)
        {
            _userPreferences = await _userPreferencesManager.LoadUserPreferences();
            if (_userPreferences != null)
            {
                IsDarkMode = _userPreferences.IsDarkMode;
            }
            else
            {
                IsDarkMode = isDarkModeDefaultTheme;
                _userPreferences = new UserPreferences { IsDarkMode = IsDarkMode };
                await _userPreferencesManager.SaveUserPreferences(_userPreferences);
            }
        }

        public event EventHandler MajorUpdateOccured;

        private void OnMajorUpdateOccured() => MajorUpdateOccured?.Invoke(this, EventArgs.Empty);

        public async Task ToggleDarkMode()
        {
            IsDarkMode = !IsDarkMode;
            _userPreferences.IsDarkMode = IsDarkMode;
            await _userPreferencesManager.SaveUserPreferences(_userPreferences);
            OnMajorUpdateOccured();
        }

        public void SetBaseTheme(MudTheme theme)
        {
            CurrentTheme = theme;
            OnMajorUpdateOccured();
        }
    }
}

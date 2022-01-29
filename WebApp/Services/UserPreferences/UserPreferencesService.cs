using System.Threading.Tasks;
using Blazored.LocalStorage;
using MudBlazor;

namespace WebApp.Services.UserPreferences
{
    public interface IUserPreferencesService
    {
        Task<MudTheme> GetCurrentThemeAsync();

        Task<bool> ToggleDarkModeAsync();
    }
    
    public class UserPreferencesService : IUserPreferencesService
    {
        private readonly ILocalStorageService _localStorageService;
        private const string Key = "userPreferences";

        public UserPreferencesService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<bool> ToggleDarkModeAsync()
        {
            var preference = await GetPreference();
            if (preference != null)
            {
                preference.IsDarkMode = !preference.IsDarkMode;
                await SetPreference(preference);
                return !preference.IsDarkMode;
            }

            return false;
        }

        public async Task<MudTheme> GetCurrentThemeAsync()
        {
            var preference = await GetPreference();
            if (preference != null)
            {
                if (preference.IsDarkMode == true) return Theme.DarkTheme;
            }
            return Theme.DefaultTheme;
        }

        public async Task<UserPreferences> GetPreference()
        {
            return await _localStorageService.GetItemAsync<UserPreferences>(Key) ?? new UserPreferences();
        }

        public async Task SetPreference(UserPreferences preference)
        {
            await _localStorageService.SetItemAsync(Key, preference);
        }
    }
}

using Blazored.LocalStorage;

namespace Client.Infrastructure.Preferences;

public class UserPreferencesManager : IUserPreferencesManager
{
    private readonly ILocalStorageService _localStorageService;
    private const string Key = "userPreferences";

    public UserPreferencesManager(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task SaveUserPreferences(UserPreferences userPreferences)
    {
        await _localStorageService.SetItemAsync(Key, userPreferences);
    }

    public async Task<UserPreferences> LoadUserPreferences()
    {
        return await _localStorageService.GetItemAsync<UserPreferences>(Key);
    }
}

namespace Client.Infrastructure.Preferences
{
    public interface IUserPreferencesManager
    {
        Task SaveUserPreferences(UserPreferences userPreferences);

        Task<UserPreferences> LoadUserPreferences();
    }
}

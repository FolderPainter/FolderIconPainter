using Microsoft.UI.Xaml;

namespace FIP.App.Helpers.Settings;

public partial class SettingsHelper : ObservableSettings
{
    private static readonly SettingsHelper instance = new(SettingsProviderFactory.CreateProvider());
    public static SettingsHelper Current => instance;

    private SettingsHelper(ISettingsProvider provider)
        : base(provider)
    {
    }
    public const int MaxRecentlyVisitedSamples = 7;

    public ElementTheme SelectedAppTheme
    {
        get => GetOrCreateDefault<ElementTheme>(ElementTheme.Default);
        set => Set(value);
    }

    public bool IsLeftMode
    {
        get => GetOrCreateDefault<bool>(true);
        set => Set(value);
    }

    public bool IsShowCopyLinkTeachingTip
    {
        get => GetOrCreateDefault<bool>(true);
        set => Set(value);
    }

    public bool IsFirstRun
    {
        get => GetOrCreateDefault<bool>(true);
        set => Set(value);
    }
}

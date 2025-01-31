namespace CoinFlow.Utils;

public static class Settings
{
    public static T GetSettings<T>(this IConfiguration configuration, string settingsKey)
    {
        return configuration.GetRequiredSection(settingsKey).Get<T>()
               ?? throw new InvalidOperationException("Settings are not configured.");
    }
}

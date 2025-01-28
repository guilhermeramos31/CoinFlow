namespace CoinFlow.Utils;

public static class Settings
{
    public static T GetSettings<T>(string settingsKey, IConfiguration configuration)
    {
        return configuration.GetRequiredSection(settingsKey).Get<T>()
               ?? throw new InvalidOperationException("Settings are not configured.");
    }
}
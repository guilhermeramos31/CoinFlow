namespace CoinFlow.Infrastructure.Configurations.Settings;

public class JwtSetting
{
    public ICollection<string> Issuers { get; set; } = Array.Empty<string>();
    public string Audience { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public int ExpireMinutes { get; set; }
}

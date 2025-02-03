namespace CoinFlow.Models.Tokens;

public class Tokens
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;

    public DateTime ExpiresAt { get; init; } = DateTime.UtcNow;
}

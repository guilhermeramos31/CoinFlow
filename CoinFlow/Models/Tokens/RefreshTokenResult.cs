namespace CoinFlow.Models.Tokens;

public class RefreshTokenResult
{
    public string RefreshToken { get; init; } = string.Empty;
    public string RefreshTokenHash { get; init; } = string.Empty;

    public DateTime ExpiresAt { get; init; } = DateTime.UtcNow;
}

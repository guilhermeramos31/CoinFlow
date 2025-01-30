namespace CoinFlow.Services.Interfaces;

using Models.UserEntity;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(User user);
    Task<string> RefreshTokenAsync(User user);
}

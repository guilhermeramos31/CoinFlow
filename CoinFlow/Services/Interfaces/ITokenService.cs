namespace CoinFlow.Services.Interfaces;

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Models.Tokens;
using Models.UserEntity;

public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(User user);
    RefreshTokenResult GenerateRefreshToken();

    Task<string> CreateUserTokenAsync(User user, RefreshTokenResult refreshToken);
    ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters);
}

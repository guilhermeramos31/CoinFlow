namespace CoinFlow.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using BCrypt.Net;
using Infrastructure.Configurations.Settings;
using Infrastructure.Managers;
using Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Tokens;
using Models.UserEntity;
using Utils;

public class TokenService(
    UowManager uowManager,
    IHttpContextAccessor contextAccessor,
    IOptions<JwtSetting> jwtSetting,
    JwtSecurityTokenHandler tokenHandler)
    : ITokenService
{
    public async Task<string> GenerateAccessTokenAsync(User user)
    {
        var jwt = jwtSetting.Value;
        var issuerOrigin = contextAccessor.Get().Request.Headers.Origin;

        var claims = new List<Claim>
        {
            new(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var userRoles = await uowManager.UserManager.GetRolesAsync(user);
        foreach (var role in userRoles) claims.Add(new Claim("role", role));

        var token = new JwtSecurityToken(
            issuer: issuerOrigin,
            claims: claims,
            audience: jwt.Audience,
            expires: DateTime.UtcNow.AddMinutes(jwt.ExpireMinutes * 2),
            signingCredentials: new(Encoding.SymmetricSecurityKey(jwt.Secret), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshTokenResult GenerateRefreshToken()
    {
        var randomNumber = RandomNumberGenerator.GetBytes(64);
        var refreshToken = Convert.ToBase64String(randomNumber);

        return new()
        {
            RefreshToken = refreshToken,
            RefreshTokenHash = BCrypt.HashPassword(refreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
    }

    public async Task<string> CreateUserTokenAsync(User user, RefreshTokenResult refreshToken)
    {
        var jwt = jwtSetting.Value;
        var token = await uowManager.UserManager.SetAuthenticationTokenAsync(user, jwt.Audience, "RefreshToken",
            refreshToken.RefreshTokenHash);
        if (!token.Succeeded)
        {
            throw new BadHttpRequestException("Failed to save Refresh Token to database.");
        }

        return refreshToken.RefreshToken;
    }

    public ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters)
    {
        var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return claimsPrincipal;
    }
}

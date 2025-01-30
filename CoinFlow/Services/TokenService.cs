namespace CoinFlow.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Infrastructure.Configurations.Settings;
using Infrastructure.Managers;
using Interfaces;
using Microsoft.IdentityModel.Tokens;
using Models.UserEntity;
using Utils;

public class TokenService(
    UowManager uowManager,
    IHttpContextAccessor contextAccessor,
    JwtSetting jwtSetting)
    : ITokenService
{
    public async Task<string> GenerateTokenAsync(User user)
    {
        if (string.IsNullOrEmpty(jwtSetting.Secret))
            throw new ArgumentException("JWT secret key is missing or invalid.");

        var issuerOrigin = contextAccessor.Get().Request.Headers.Origin;
        var issuers = jwtSetting.Issuers.Where(issuer => !string.IsNullOrEmpty(issuer)).ToList();
        if (string.IsNullOrWhiteSpace(issuerOrigin) ||
            !issuers.Any(i => string.Equals(i, issuerOrigin, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("Issuer invalid");

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToLongDateString(), ClaimValueTypes.Integer64)
        };

        var userRoles = await uowManager.UserManager.GetRolesAsync(user);
        foreach (var role in userRoles) claims.Add(new Claim(ClaimTypes.Role, role));

        try
        {
            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: issuerOrigin,
                claims: claims,
                audience: jwtSetting.Audience,
                expires: DateTime.UtcNow.AddMinutes(jwtSetting.ExpireMinutes * 2),
                signingCredentials: new(Encoding.GetBytes(jwtSetting.Secret), SecurityAlgorithms.HmacSha256)
            ));
        }
        catch (Exception exception)
        {
            throw new ApplicationException("Failed to generate JWT token.", exception);
        }
    }

    public Task<string> RefreshTokenAsync(User user)
    {
        throw new NotImplementedException();
    }
}

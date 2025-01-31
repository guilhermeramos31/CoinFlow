namespace CoinFlow.Utils;

using Infrastructure.Configurations.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public static class TokenValidations
{
    public static TokenValidationParameters ToTokenValidationParams(this IOptions<JwtSetting> options)
    {
        return new()
        {
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidIssuers = options.Value.Issuers,
            ValidAudience = options.Value.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = Encoding.SymmetricSecurityKey(options.Value.Secret)
        };
    }
}

namespace CoinFlow.Infrastructure.Configurations;

using Microsoft.IdentityModel.Tokens;
using Settings;
using Utils;
using Encoding = Utils.Encoding;

public static class AuthConfig
{
    public static void BuildAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtPayload = configuration.GetSettings<JwtSetting>("JwtSettings");

        services.AddAuthentication().AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                SaveSigninToken = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,
                ValidIssuers = jwtPayload.Issuers,
                ValidAudience = jwtPayload.Audience,
                IssuerSigningKey = Encoding.GetBytes(jwtPayload.Secret)
            };
            options.Events = new()
            {
                OnAuthenticationFailed = context => throw new BadHttpRequestException(context.Exception.Message),
            };
        });
        services.AddAuthorization();
    }
}

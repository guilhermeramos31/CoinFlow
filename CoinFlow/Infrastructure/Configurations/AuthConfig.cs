namespace CoinFlow.Infrastructure.Configurations;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
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
                ValidateIssuerSigningKey = true,
                ValidIssuers = jwtPayload.Issuers,
                ValidAudience = jwtPayload.Audience,
                IssuerSigningKey = Encoding.SymmetricSecurityKey(jwtPayload.Secret)
            };
            options.Events = new()
            {
                OnAuthenticationFailed = context =>
                {
                    context.NoResult();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "text/plain";
                    return context.Response.WriteAsync("Unauthorized: Invalid token.");
                }
            };
        });
        services.AddAuthorization();
    }
}

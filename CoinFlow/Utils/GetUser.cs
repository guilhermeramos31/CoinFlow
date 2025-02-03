namespace CoinFlow.Utils;

using System.Security.Claims;
using Infrastructure.Configurations.Settings;
using Infrastructure.Managers;
using Microsoft.Extensions.Options;
using Models.UserEntity;
using Services.Interfaces;

public static class CurrentUser
{
    public static async Task<User> GetUser(this IHttpContextAccessor accessor, IOptions<JwtSetting> jwtSetting,
        ITokenService tokenService, UowManager uowManager)
    {
        var token = accessor.GetToken();

        var tokenValidation = tokenService.ValidateToken(token, jwtSetting.ToTokenValidationParams());
        if (tokenValidation == null) throw new UnauthorizedAccessException("User not Authenticated.");

        var idClaims = tokenValidation.FindFirstValue(ClaimTypes.NameIdentifier);
        if (idClaims == null) throw new BadHttpRequestException("Claims invalid.");

        var userById = await uowManager.UserManager.FindByIdAsync(idClaims);
        if (userById == null) throw new BadHttpRequestException("User not found");

        return userById;
    }
}

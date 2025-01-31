namespace CoinFlow.Services;

using System.Security.Claims;
using AutoMapper;
using Infrastructure.Configurations.Settings;
using Infrastructure.Managers;
using Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Models.RoleEntity.Enums;
using Models.UserEntity;
using Models.UserEntity.Dto;
using Utils;

public class UserService(
    IMapper mapper,
    UowManager uowManager,
    IHttpContextAccessor accessor,
    ITokenService tokenService,
    IOptions<JwtSetting> jwtSetting) : IUserService
{
    public async Task<UserResponse> CreateAsync(UserRequest request)
    {
        var userNameExist = await uowManager.UserManager.FindByNameAsync(request.UserName);
        if (userNameExist != null) throw new BadHttpRequestException($"User {request.UserName} already exists.");

        var userEmailExist = await uowManager.UserManager.FindByEmailAsync(request.Email);
        if (userEmailExist != null) throw new BadHttpRequestException($"User {request.Email} already exists.");

        var user = mapper.Map<User>(request);
        var resultUser = await uowManager.UserManager.CreateAsync(user, request.Password);
        if (resultUser.Errors.Any())
            throw new BadHttpRequestException(
                $"Failed to create user: {string.Join(", ", resultUser.Errors.Select(x => x.Description))}");

        var resultUserRole = await uowManager.UserManager.AddToRoleAsync(user, RoleType.User.ToString());
        if (resultUserRole.Errors.Any())
        {
            await uowManager.UserManager.DeleteAsync(user);
            throw new BadHttpRequestException(
                $"Failed to create user: {string.Join(", ", resultUserRole.Errors.Select(x => x.Description))}");
        }

        return mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse> CurrentUser()
    {
        var token = accessor.GetToken();

        var jwt = jwtSetting.Value;
        var validateParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidIssuers = jwt.Issuers,
            ValidAudience = jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = Encoding.SymmetricSecurityKey(jwt.Secret)
        };

        var tokenValidation = tokenService.ValidateToken(token, validateParameters);
        if (tokenValidation == null) throw new UnauthorizedAccessException("User not Authenticated.");

        var emailClaims = tokenValidation.FindFirstValue(ClaimTypes.Email);
        var idClaims = tokenValidation.FindFirstValue(ClaimTypes.NameIdentifier);
        if (emailClaims == null || idClaims == null) throw new BadHttpRequestException("Claims invalid.");

        var userById = await uowManager.UserManager.FindByIdAsync(idClaims);
        var userByEmail = await uowManager.UserManager.FindByEmailAsync(emailClaims);
        if (userByEmail == null || userById == null) throw new BadHttpRequestException("User not found");
        if (userByEmail.Id != userById.Id) throw new BadHttpRequestException("claims do not match");

        return mapper.Map<UserResponse>(userById);
    }
}

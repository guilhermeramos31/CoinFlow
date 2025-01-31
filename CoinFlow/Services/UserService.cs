namespace CoinFlow.Services;

using System.Security.Claims;
using AutoMapper;
using Infrastructure.Configurations.Settings;
using Infrastructure.Managers;
using Interfaces;
using Microsoft.Extensions.Options;
using Models.RoleEntity.Enums;
using Models.UserEntity;
using Models.UserEntity.Dto;
using Utils;

public class UserService(
    IMapper mapper,
    UowManager uowManager,
    IHttpContextAccessor accessor,
    ITokenService tokenService,
    IOptions<JwtSetting> jwtSetting,
    IWalletService walletService) : IUserService
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

        await walletService.CreateWallet(user.Id);

        return mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse> CurrentUser()
    {
        return mapper.Map<UserResponse>(await accessor.GetUser(jwtSetting, tokenService, uowManager));
    }
}

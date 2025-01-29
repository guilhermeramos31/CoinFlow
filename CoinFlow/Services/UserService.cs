namespace CoinFlow.Services;

using AutoMapper;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Models.RoleEntity.Enums;
using Models.UserEntity;
using Models.UserEntity.Dto;

public class UserService(IMapper mapper, UserManager<User> userManager) : IUserService
{
    public async Task<UserResponse> CreateAsync(UserRequest request)
    {
        var userNameExist = await userManager.FindByNameAsync(request.UserName);
        if (userNameExist != null) throw new BadHttpRequestException($"User {request.UserName} already exists.");

        var userEmailExist = await userManager.FindByEmailAsync(request.Email);
        if (userEmailExist != null) throw new BadHttpRequestException($"User {request.Email} already exists.");

        var user = mapper.Map<User>(request);
        var resultUser = await userManager.CreateAsync(user, request.Password);
        if (resultUser.Errors.Any())
            throw new BadHttpRequestException(
                $"Failed to create user: {string.Join(", ", resultUser.Errors.Select(x => x.Description))}");

        var resultUserRole = await userManager.AddToRoleAsync(user, RoleType.User.ToString());
        if (resultUserRole.Errors.Any())
        {
            await userManager.DeleteAsync(user);
            throw new BadHttpRequestException(
                $"Failed to create user: {string.Join(", ", resultUserRole.Errors.Select(x => x.Description))}");
        }

        return mapper.Map<UserResponse>(user);
    }
}

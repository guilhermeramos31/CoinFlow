namespace CoinFlow.Services;

using AutoMapper;
using Infrastructure.Managers;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Tokens;
using Models.UserEntity.Dto;
using LoginRequest = Models.UserEntity.Dto.LoginRequest;

public class AuthService(UowManager uowManager, ITokenService tokenService, IMapper mapper) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        var user = await uowManager.UserManager.Users.FirstOrDefaultAsync(user => user.Email == loginRequest.Email);
        if (user == null) throw new BadHttpRequestException("Invalid username");

        var password = await uowManager.UserManager.CheckPasswordAsync(user, loginRequest.Password);
        if (!password) throw new BadHttpRequestException("Invalid password");

        var refreshToken = tokenService.GenerateRefreshToken();
        return new()
        {
            Success = true,
            Message = $"{user.UserName} is logged",
            User = mapper.Map<UserResponse>(user),
            Tokens = new Tokens
            {
                AccessToken = await tokenService.GenerateAccessTokenAsync(user),
                RefreshToken = await tokenService.CreateUserTokenAsync(user, refreshToken),
                ExpiresAt = refreshToken.ExpiresAt
            }
        };
    }
}

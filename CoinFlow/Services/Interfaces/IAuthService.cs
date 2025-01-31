namespace CoinFlow.Services.Interfaces;

using Models.UserEntity.Dto;
using LoginRequest = Models.UserEntity.Dto.LoginRequest;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
}

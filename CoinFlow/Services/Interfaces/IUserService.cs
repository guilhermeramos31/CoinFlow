namespace CoinFlow.Services.Interfaces;

using Models.UserEntity.Dto;

public interface IUserService
{
    Task<UserResponse> CreateAsync(UserRequest request);
    Task<UserResponse> CurrentUser();
}

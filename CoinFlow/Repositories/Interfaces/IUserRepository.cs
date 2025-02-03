namespace CoinFlow.Repositories.Interfaces;

using Models.UserEntity;
using Services;

public interface IUserRepository
{
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserById(Guid id);
}

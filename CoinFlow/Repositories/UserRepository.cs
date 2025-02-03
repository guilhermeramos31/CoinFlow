namespace CoinFlow.Repositories;

using Infrastructure.Data.Contexts;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.UserEntity;
using Models.WalletEntity;

public class UserRepository(CoinFlowContext dbContext) : IUserRepository
{
    public async Task<User?> GetUserByEmail(string email)
    {
        return await dbContext.Users.Include(user => user.Wallet).FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await dbContext.Users.Include(user => user.Wallet).FirstOrDefaultAsync(u => u.Id == id);
    }
}

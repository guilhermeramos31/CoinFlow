namespace CoinFlow.Repositories;

using Infrastructure.Data.Contexts;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.WalletEntity;

public class WalletRepository(CoinFlowContext dbContext) : IWalletRepository
{
    public async Task<Wallet?> FirstOrDefaultAsync(Guid userId)
    {
        return await dbContext.Wallets.FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public void UpdateRange(Wallet sender, Wallet receiver)
    {
        dbContext.Wallets.UpdateRange(sender, receiver);
        dbContext.SaveChanges();
    }
}

namespace CoinFlow.Repositories.Interfaces;

using Models.WalletEntity;

public interface IWalletRepository
{
    Task<Wallet?> FirstOrDefaultAsync(Guid userId);
    void UpdateRange(Wallet sender, Wallet receiver);
}

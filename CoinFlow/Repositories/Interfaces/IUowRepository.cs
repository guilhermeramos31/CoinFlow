namespace CoinFlow.Repositories.Interfaces;
public interface IUowRepository
{
    IUserRepository UserRepository { get; }
    IWalletRepository WalletRepository { get; }
    ITransactionHistoryRepository TransactionHistory { get; }
    void Commit();
    Task CommitAsync();
    void Rollback();
    void BeginTransaction();
    Task RollbackAsync();
}

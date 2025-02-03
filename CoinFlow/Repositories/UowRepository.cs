namespace CoinFlow.Repositories;

using Infrastructure.Data.Contexts;
using Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

public class UowRepository(CoinFlowContext dbContext) : IUowRepository
{
    private IDbContextTransaction? _transaction = null;
    private ITransactionHistoryRepository? _transactionHistoryRepository = null;
    private IWalletRepository? _walletRepository = null;
    private IUserRepository? _userRepository = null;

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(dbContext);
    public IWalletRepository WalletRepository => _walletRepository ??= new WalletRepository(dbContext);

    public ITransactionHistoryRepository TransactionHistory =>
        _transactionHistoryRepository ??= new TransactionHistoryRepository(dbContext);

    public void Dispose()
    {
        _transaction?.Dispose();
    }

    public async Task DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }
    }

    public void Commit()
    {
        dbContext.SaveChanges();
        _transaction?.Commit();
    }

    public async Task CommitAsync()
    {
        await dbContext.SaveChangesAsync();
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
        }
    }

    public void Rollback()
    {
        _transaction?.Rollback();
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
        }
    }

    public void BeginTransaction()
    {
        _transaction = dbContext.Database.BeginTransaction();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await dbContext.Database.BeginTransactionAsync();
    }
}

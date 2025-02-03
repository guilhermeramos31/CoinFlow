namespace CoinFlow.Repositories.Interfaces;

using Models.TransactionHistoryEntity;

public interface ITransactionHistoryRepository
{
    Task CreateAsync(TransactionHistory transactionHistory);
    IQueryable<TransactionHistoryDto> GetUserTransactions(Guid userId, DateTime? startDate, DateTime? endDate);
}

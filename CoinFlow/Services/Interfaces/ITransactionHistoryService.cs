namespace CoinFlow.Services.Interfaces;

using Models.TransactionHistoryEntity;
using Models.UserEntity;

public interface ITransactionHistoryService
{
    Task CreateTransactionHistory(User user, User receiver, decimal amount);
    Task<IEnumerable<TransactionHistoryDto>> TransferTransactionHistory(DateTime? initDate, DateTime? endDate);
}

namespace CoinFlow.Services.Interfaces;

using Models.TransactionHistoryEntity;
using Models.UserEntity;

public interface ITransactionHistoryService
{
    Task CreateAsync(User user, User receiver, decimal amount);
    Task<IEnumerable<TransactionHistoryDto>> TransferHistory(DateTime? initDate, DateTime? endDate);
}

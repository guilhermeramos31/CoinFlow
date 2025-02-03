namespace CoinFlow.Repositories;

using Infrastructure.Data.Contexts;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.TransactionHistoryEntity;

public class TransactionHistoryRepository(CoinFlowContext dbContext) : ITransactionHistoryRepository
{
    public async Task CreateAsync(TransactionHistory transactionHistory)
    {
        await dbContext.TransactionHistory.AddAsync(transactionHistory);
        await dbContext.SaveChangesAsync();
    }

    public IQueryable<TransactionHistoryDto> GetUserTransactions(Guid userId, DateTime? startDate, DateTime? endDate)
    {
        var query = dbContext.TransactionHistory
            .Where(transactionHistory =>
                transactionHistory.UserId.Equals(userId) || transactionHistory.ReceiverId.Equals(userId));

        if (startDate.HasValue && endDate.HasValue && startDate.Value != DateTime.MinValue &&
            endDate.Value != DateTime.MinValue)
            query = query.Where(
                t => t.CreateAt.Date >= startDate.Value.Date &&
                     t.CreateAt.Date <= endDate.Value.Date);

        query = query
            .Include(transactionHistory => transactionHistory.Receiver)
            .Include(transactionHistory => transactionHistory.User).OrderByDescending(t => t.CreateAt);

        return query.Select(transaction => new TransactionHistoryDto
        {
            Id = transaction.Id,
            Value = transaction.Value,
            SenderName = transaction.User.Name,
            ReceiverName = transaction.Receiver.Name
        });
    }
}

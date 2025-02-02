namespace CoinFlow.Services;

using Infrastructure.Configurations.Settings;
using Infrastructure.Data.Contexts;
using Infrastructure.Managers;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models.TransactionHistoryEntity;
using Models.UserEntity;
using Utils;

public class TransactionHistoryService(
    CoinFlowContext dbContext,
    IHttpContextAccessor accessor,
    IOptions<JwtSetting> jwtSetting,
    ITokenService tokenService,
    UowManager uowManager) : ITransactionHistoryService
{
    public async Task CreateTransactionHistory(User user, User receiver, decimal amount)
    {
        await dbContext.TransactionHistory.AddAsync(new()
        {
            UserId = user.Id,
            User = user,
            ReceiverId = receiver.Id,
            Receiver = receiver,
            Value = amount
        });
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<TransactionHistoryDto>> TransferTransactionHistory(DateTime? initDate,
        DateTime? endDate)
    {
        var user = await accessor.GetUser(jwtSetting, tokenService, uowManager);
        if (user == null) throw new BadHttpRequestException("User doesn't exist");

        if (initDate.HasValue && endDate.HasValue && initDate.Value > endDate.Value)
        {
            throw new BadHttpRequestException("initDate must be less than or equal to endDate");
        }

        var query = dbContext.TransactionHistory
            .Where(transactionHistory =>
                transactionHistory.UserId.Equals(user.Id) || transactionHistory.ReceiverId.Equals(user.Id));

        if (initDate.HasValue && endDate.HasValue && initDate.Value != DateTime.MinValue && endDate.Value != DateTime.MinValue)
            query = query.Where(
                t => t.CreateAt.Date >= initDate.Value.Date &&
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

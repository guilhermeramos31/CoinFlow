namespace CoinFlow.Services;

using Infrastructure.Configurations.Settings;
using Infrastructure.Managers;
using Interfaces;
using Microsoft.Extensions.Options;
using Models.TransactionHistoryEntity;
using Models.UserEntity;
using Repositories.Interfaces;
using Utils;

public class TransactionHistoryService(
    IUowRepository uowRepository,
    IHttpContextAccessor accessor,
    IOptions<JwtSetting> jwtSetting,
    ITokenService tokenService,
    UowManager uowManager) : ITransactionHistoryService
{
    public async Task CreateAsync(User user, User receiver, decimal amount)
    {
        await uowRepository.TransactionHistory.CreateAsync(new()
        {
            UserId = user.Id,
            User = user,
            ReceiverId = receiver.Id,
            Receiver = receiver,
            Value = amount
        });
    }

    public async Task<IEnumerable<TransactionHistoryDto>> TransferHistory(DateTime? startDate,
        DateTime? endDate)
    {
        var user = await accessor.GetUser(jwtSetting, tokenService, uowManager);
        if (user == null) throw new BadHttpRequestException("User doesn't exist");
        if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
            throw new BadHttpRequestException("initDate must be less than or equal to endDate");

        return uowRepository.TransactionHistory.GetUserTransactions(user.Id, startDate, endDate);
    }
}

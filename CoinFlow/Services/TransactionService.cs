namespace CoinFlow.Services;

using Infrastructure.Configurations.Settings;
using Infrastructure.Managers;
using Interfaces;
using Microsoft.Extensions.Options;
using Models.TransactionHistoryEntity;
using Models.TransferEntity.Dto;
using Models.UserEntity;
using Repositories.Interfaces;
using Utils;

public class TransactionService(
    UowManager uowManager,
    ITokenService tokenService,
    IHttpContextAccessor accessor,
    IOptions<JwtSetting> jwtSetting,
    IUowRepository uowRepository
) : ITransactionService
{
    public async Task<TransferResponse> TransferAsync(TransferRequest request)
    {
        var user = await accessor.GetUser(jwtSetting, tokenService, uowManager);
        user = await uowRepository.UserRepository.GetUserById(user.Id);
        if (user == null) throw new BadHttpRequestException("User not found");
        if (user.Email == request.receiver) throw new BadHttpRequestException("You cannot transfer yourself");
        if (user.Wallet.Balance < request.amount) throw new BadHttpRequestException("Not enough balance");

        var receiver = await uowRepository.UserRepository.GetUserByEmail(request.receiver);
        if (receiver == null) throw new BadHttpRequestException("Receiver doesn't exist");

        user.Wallet.Balance -= request.amount;
        receiver.Wallet.Balance += request.amount;

        uowRepository.WalletRepository.UpdateRange(user.Wallet, receiver.Wallet);
        await CreateAsync(user, receiver, request.amount);

        return new()
        {
            receiverName = receiver.Name,
            amount = request.amount
        };
    }

    private async Task CreateAsync(User user, User receiver, decimal amount)
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

    public async Task<IEnumerable<TransactionHistoryDto>> History(DateTime? startDate,
        DateTime? endDate)
    {
        var user = await accessor.GetUser(jwtSetting, tokenService, uowManager);
        if (user == null) throw new BadHttpRequestException("User doesn't exist");
        if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
            throw new BadHttpRequestException("initDate must be less than or equal to endDate");

        return uowRepository.TransactionHistory.GetUserTransactions(user.Id, startDate, endDate);
    }
}

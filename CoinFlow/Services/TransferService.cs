namespace CoinFlow.Services;

using Infrastructure.Configurations.Settings;
using Infrastructure.Data.Contexts;
using Infrastructure.Managers;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models.TransferEntity.Dto;
using Utils;

public class TransferService(
    UowManager uowManager,
    ITokenService tokenService,
    IHttpContextAccessor accessor,
    IOptions<JwtSetting> jwtSetting,
    CoinFlowContext dbContext,
    ITransactionHistoryService transactionHistoryService
) : ITransferService
{
    public async Task<TransferResponse> TransferAsync(TransferRequest request)
    {
        var user = await accessor.GetUser(jwtSetting, tokenService, uowManager);
        var userWallet = await dbContext.Wallets.FirstOrDefaultAsync(u => u.UserId == user.Id);
        if (userWallet == null) throw new BadHttpRequestException("Wallet doesn't exist");
        if (userWallet.Balance < request.amount) throw new BadHttpRequestException("Not enough balance");

        var receiver = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.receiver);
        if (receiver == null) throw new BadHttpRequestException("Recipient doesn't exist");
        var receiverWallet = await dbContext.Wallets.FirstOrDefaultAsync(u => u.UserId == receiver.Id);
        if (receiverWallet == null) throw new BadHttpRequestException("Recipient doesn't exist");

        if (user.Email == request.receiver) throw new BadHttpRequestException("You cannot transfer yourself");

        user.Wallet.Balance -= request.amount;
        receiverWallet.Balance += request.amount;

        dbContext.Wallets.UpdateRange(userWallet, receiverWallet);

        await transactionHistoryService.CreateTransactionHistory(user, receiver, request.amount);

        await dbContext.SaveChangesAsync();

        return new()
        {
            receiverName = receiver.Name,
            amount = request.amount
        };
    }
}

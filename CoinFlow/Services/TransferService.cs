namespace CoinFlow.Services;

using Infrastructure.Configurations.Settings;
using Infrastructure.Managers;
using Interfaces;
using Microsoft.Extensions.Options;
using Models.TransferEntity.Dto;
using Repositories.Interfaces;
using Utils;

public class TransferService(
    UowManager uowManager,
    ITokenService tokenService,
    IHttpContextAccessor accessor,
    IOptions<JwtSetting> jwtSetting,
    IUowRepository uowRepository,
    ITransactionHistoryService transactionHistoryService
) : ITransferService
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
        await transactionHistoryService.CreateAsync(user, receiver, request.amount);

        return new()
        {
            receiverName = receiver.Name,
            amount = request.amount
        };
    }
}

namespace CoinFlow.Services;

using AutoMapper;
using Infrastructure.Configurations.Settings;
using Infrastructure.Data.Contexts;
using Infrastructure.Managers;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models.UserEntity.Dto;
using Models.WalletEntity;
using Models.WalletEntity.Dto;
using Utils;

public class WalletService(
    CoinFlowContext context,
    IMapper mapper,
    IHttpContextAccessor accessor,
    UowManager uowManager,
    ITokenService tokenService,
    IOptions<JwtSetting>
        jwtSetting) : IWalletService
{
    public async Task CreateWallet(Guid userId)
    {
        await context.Wallets.AddAsync(new Wallet() { UserId = userId });
        await context.SaveChangesAsync();
    }

    public async Task<WalletResponse> Deposit(decimal request)
    {
        var wallet = await GetWallet();
        if (request <= 0) throw new BadHttpRequestException("Insufficient amount");
        wallet.Balance += request;
        context.Wallets.Update(wallet);
        await context.SaveChangesAsync();

        return new()
        {
            User = mapper.Map<UserResponse>(wallet.User),
            Balance = wallet.Balance
        };
    }

    public async Task<WalletResponse> Withdrawal(decimal request)
    {
        var wallet = await GetWallet();
        if (wallet.Balance < request) throw new BadHttpRequestException("Insufficient balance");
        wallet.Balance -= request;
        context.Wallets.Update(wallet);
        await context.SaveChangesAsync();

        return new()
        {
            User = mapper.Map<UserResponse>(wallet.User),
            Balance = wallet.Balance
        };
    }

    private async Task<Wallet> GetWallet()
    {
        var user = await accessor.GetUser(jwtSetting, tokenService, uowManager);
        var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == user.Id);
        if (wallet == null) throw new BadHttpRequestException("Wallet not found");

        return wallet;
    }

    public async Task<decimal> GetBalance()
    {
        var user = await accessor.GetUser(jwtSetting, tokenService, uowManager);
        var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == user.Id);
        if (wallet == null) throw new BadHttpRequestException("Wallet not found");

        return wallet.Balance;
    }
}

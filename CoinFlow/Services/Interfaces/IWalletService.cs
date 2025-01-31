namespace CoinFlow.Services.Interfaces;

using Models.WalletEntity.Dto;

public interface IWalletService
{
    Task CreateWallet(Guid userId);
    public Task<WalletResponse> Deposit(decimal request);
    public Task<WalletResponse> Withdrawal(decimal request);
}

namespace CoinFlow.Services.Interfaces;

using Models.WalletEntity.Dto;

public interface IWalletService
{
    public Task<WalletResponse> Deposit(decimal request);
    public Task<WalletResponse> Withdrawal(decimal request);
    Task<WalletResponse> GetBalance();
}

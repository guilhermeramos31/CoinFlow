namespace CoinFlow.Models.WalletEntity;

using BaseEntity;
using UserEntity;

public class Wallet : BaseEntity
{
    public decimal Balance { get; set; } = decimal.Zero;

    public Guid UserId { get; set; }
    public User User { get; set; }
}

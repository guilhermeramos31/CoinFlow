namespace CoinFlow.Models.WalletEntity.Dto;

using UserEntity.Dto;

public class WalletResponse
{
    public UserResponse User { get; set; }
    public decimal Balance { get; set; }
}

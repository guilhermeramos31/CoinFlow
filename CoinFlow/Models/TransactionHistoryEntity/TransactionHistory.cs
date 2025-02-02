namespace CoinFlow.Models.TransactionHistoryEntity;

using BaseEntity;
using UserEntity;

public class TransactionHistory : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid ReceiverId { get; set; }
    public User Receiver { get; set; }
    public decimal Value { get; set; }
}

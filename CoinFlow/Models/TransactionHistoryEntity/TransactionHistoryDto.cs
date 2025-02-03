namespace CoinFlow.Models.TransactionHistoryEntity;

public class TransactionHistoryDto
{
    public Guid Id { get; set; }
    public decimal Value { get; set; }
    public string SenderName { get; set; }
    public string ReceiverName { get; set; }
}

namespace CoinFlow.Models.TransferEntity.Dto;

public class TransferResponse
{
    public string receiverName { get; set; } = string.Empty;
    public decimal amount { get; set; } = decimal.Zero;
}

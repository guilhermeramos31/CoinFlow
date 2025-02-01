namespace CoinFlow.Models.TransferEntity.Dto;

public class TransferRequest
{
    public required string receiver { get; set; } = string.Empty;
    public required decimal amount { get; set; } = decimal.Zero;
}

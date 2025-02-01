namespace CoinFlow.Services.Interfaces;

using Models.TransferEntity.Dto;

public interface ITransferService
{
    Task<TransferResponse> TransferAsync(TransferRequest request);
}

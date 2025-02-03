namespace CoinFlow.Services.Interfaces;

using Models.TransactionHistoryEntity;
using Models.TransferEntity.Dto;

public interface ITransactionService
{
    Task<TransferResponse> TransferAsync(TransferRequest request);
    Task<IEnumerable<TransactionHistoryDto>> History(DateTime? initDate, DateTime? endDate);
}

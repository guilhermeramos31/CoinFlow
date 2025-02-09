﻿namespace CoinFlow.Models.UserEntity;

using Microsoft.AspNetCore.Identity;
using TransactionHistoryEntity;
using WalletEntity;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    public Wallet Wallet { get; set; } = new();
    public List<TransactionHistory> SentTransactions { get; set; } = new();
    public List<TransactionHistory> ReceivedTransactions { get; set; } = new();
}

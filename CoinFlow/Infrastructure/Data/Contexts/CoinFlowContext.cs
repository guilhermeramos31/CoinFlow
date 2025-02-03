namespace CoinFlow.Infrastructure.Data.Contexts;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.BaseEntity;
using Models.RoleEntity;
using Models.TransactionHistoryEntity;
using Models.UserEntity;
using Models.WalletEntity;

public class CoinFlowContext(DbContextOptions<CoinFlowContext> options) : IdentityDbContext<User, Role, Guid>(options)
{
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<TransactionHistory> TransactionHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

        builder.Entity<User>()
            .HasOne(u => u.Wallet)
            .WithOne(w => w.User)
            .HasForeignKey<Wallet>(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<TransactionHistory>()
            .HasOne(t => t.User)
            .WithMany(u => u.SentTransactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TransactionHistory>()
            .HasOne(t => t.Receiver)
            .WithMany(u => u.ReceivedTransactions)
            .HasForeignKey(t => t.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TransactionHistory>()
            .Property(t => t.CreateAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.CreateAt))
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

                builder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.UpdateAt))
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            }
        }
    }
}

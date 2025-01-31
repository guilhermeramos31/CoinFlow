namespace CoinFlow.Infrastructure.Data.Contexts;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.RoleEntity;
using Models.UserEntity;
using Models.WalletEntity;

public class CoinFlowContext(DbContextOptions<CoinFlowContext> options) : IdentityDbContext<User, Role, Guid>(options)
{
    public DbSet<Wallet> Wallets { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>()
            .HasOne(u => u.Wallet)
            .WithOne(w => w.User)
            .HasForeignKey<Wallet>(w => w.UserId);
    }
}

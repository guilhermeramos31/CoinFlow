namespace CoinFlow.Infrastructure.Data.Contexts;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

public class CoinFlowContext(DbContextOptions<CoinFlowContext> options) : IdentityDbContext<User, Role, Guid>(options)
{
}

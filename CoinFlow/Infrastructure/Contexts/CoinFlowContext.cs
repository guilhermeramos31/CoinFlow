namespace CoinFlow.Infrastructure.Contexts;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

public class CoinFlowContext(DbContextOptions<CoinFlowContext> options) : IdentityDbContext<User, Role, Guid>(options)
{
}

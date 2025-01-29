namespace CoinFlow.Infrastructure.Configurations;

using Data.Contexts;
using Microsoft.AspNetCore.Identity;
using Models;

public static class IdentityConfig
{
    public static void BuildIdentity(this IServiceCollection service)
    {
        service.AddIdentity<User, Role>(employee =>
            {
                employee.Password.RequireDigit = false;
                employee.Password.RequireLowercase = false;
                employee.Password.RequireUppercase = false;
                employee.Password.RequireNonAlphanumeric = false;
            } )
            .AddRoleManager<RoleManager<Role>>()
            .AddUserManager<UserManager<User>>()
            .AddEntityFrameworkStores<CoinFlowContext>()
            .AddDefaultTokenProviders();
    }
}

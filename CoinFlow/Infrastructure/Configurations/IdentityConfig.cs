namespace CoinFlow.Infrastructure.Configurations;

using Data.Contexts;
using Microsoft.AspNetCore.Identity;
using Models.RoleEntity;
using Models.UserEntity;

public static class IdentityConfig
{
    public static void BuildIdentity(this IServiceCollection service)
    {
        service.AddIdentity<User, Role>(employee =>
            {
                employee.Password.RequireDigit = true;
                employee.Password.RequireLowercase = true;
                employee.Password.RequireUppercase = true;
                employee.Password.RequireNonAlphanumeric = true;
            } )
            .AddUserManager<UserManager<User>>()
            .AddRoleManager<RoleManager<Role>>()
            .AddEntityFrameworkStores<CoinFlowContext>()
            .AddDefaultTokenProviders();
    }
}

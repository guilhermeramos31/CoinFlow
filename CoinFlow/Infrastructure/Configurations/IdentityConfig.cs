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
                employee.Password.RequireDigit = false;
                employee.Password.RequireLowercase = false;
                employee.Password.RequireUppercase = false;
                employee.Password.RequireNonAlphanumeric = false;
            } )
            .AddUserManager<UserManager<User>>()
            .AddRoleManager<RoleManager<Role>>()
            .AddEntityFrameworkStores<CoinFlowContext>()
            .AddDefaultTokenProviders();
    }
}

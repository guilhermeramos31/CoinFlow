namespace CoinFlow.Infrastructure.Managers;

using Microsoft.AspNetCore.Identity;
using Models.RoleEntity;
using Models.UserEntity;

public class UowManager(UserManager<User> userManager, RoleManager<Role> roleManager)
{
    public UserManager<User> UserManager => userManager;
    public RoleManager<Role> RoleManager => roleManager;
}

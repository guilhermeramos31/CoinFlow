namespace CoinFlow.Infrastructure.Configurations;

using Microsoft.AspNetCore.Identity;
using Models;
using Models.Enums;

public static class RoleConfig
{
    public static async Task CreateRolesAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        foreach (var role in Enum.GetValues<RoleType>())
        {
            if (!await roleManager.RoleExistsAsync(role.ToString()))
                await roleManager.CreateAsync(new Role(role.ToString())
                {
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                });
        }
    }
}

using CoinFlow.Infrastructure.Data.Contexts;
using CoinFlow.Services.Interfaces;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var dbContextUsers = scope.ServiceProvider.GetRequiredService<CoinFlowContext>().Users.Any();
        if (dbContextUsers) return;

        var usersToCreate = new[]
        {
            new
            {
                Name = "test",
                Email = "test@test.com",
                UserName = "test",
                Password = "Test123@",
                PhoneNumber = "+5585999999999"
            },
            new
            {
                Name = "test",
                Email = "test1@test.com",
                UserName = "test1",
                Password = "Test123@",
                PhoneNumber = "+5585999999999"
            }
        };
        foreach (var user in usersToCreate)
        {
            await userService.CreateAsync(new()
            {
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber
            });
        }
    }
}

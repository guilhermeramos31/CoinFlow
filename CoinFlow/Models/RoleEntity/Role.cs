namespace CoinFlow.Models.RoleEntity;

using Microsoft.AspNetCore.Identity;

public class Role(string name) : IdentityRole<Guid>(name)
{
}

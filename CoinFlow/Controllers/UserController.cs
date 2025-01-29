using Microsoft.AspNetCore.Mvc;

namespace CoinFlow.Controllers;

using Models.UserEntity.Dto;
using Services.Interfaces;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRequest request)
    {
        return Ok(await userService.CreateAsync(request));
    }
};

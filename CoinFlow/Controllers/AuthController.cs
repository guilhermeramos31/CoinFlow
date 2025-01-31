namespace CoinFlow.Controllers;

using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using LoginRequest = Models.UserEntity.Dto.LoginRequest;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        return  Ok( await authService.LoginAsync(loginRequest));
    }
}

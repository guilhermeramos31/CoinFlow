using Microsoft.AspNetCore.Mvc;

namespace CoinFlow.Controllers;

using Models.TransferEntity.Dto;
using Models.UserEntity.Dto;
using Services.Interfaces;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService, ITransferService transferService) : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<IActionResult> Register(UserRequest request)
    {
        return Ok(await userService.CreateAsync(request));
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> Me()
    {
        return Ok(await userService.CurrentUser());
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> TransferAsync(TransferRequest request)
    {
        return Ok(await transferService.TransferAsync(request));
    }
}

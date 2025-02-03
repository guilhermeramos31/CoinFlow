namespace CoinFlow.Controllers;

using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

[ApiController]
[Route("[controller]")]
public class WalletController(IWalletService walletService) : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<IActionResult> Deposit(decimal request)
    {
        return Ok(await walletService.Deposit(request));
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Withdrawal(decimal request)
    {
        return Ok(await walletService.Withdrawal(request));
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> Balance()
    {
        return Ok(await walletService.GetBalance());
    }
}

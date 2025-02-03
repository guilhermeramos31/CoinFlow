namespace CoinFlow.Controllers;

using Microsoft.AspNetCore.Mvc;
using Models.TransferEntity.Dto;
using Services.Interfaces;

[ApiController]
[Route("[controller]")]
public class WalletController(
    IWalletService walletService,
    ITransferService transferService,
    ITransactionHistoryService transactionHistoryService) : ControllerBase
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

    [HttpPost("[action]")]
    public async Task<IActionResult> TransferAsync(TransferRequest request)
    {
        return Ok(await transferService.TransferAsync(request));
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> TransferHistory([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        return Ok(await transactionHistoryService.TransferHistory(startDate, endDate));
    }
}

namespace CoinFlow.Controllers;

using Microsoft.AspNetCore.Mvc;
using Models.TransferEntity.Dto;
using Services.Interfaces;

[ApiController]
[Route("[controller]")]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<IActionResult> TransferAsync(TransferRequest request)
    {
        return Ok(await transactionService.TransferAsync(request));
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> TransferHistory([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        return Ok(await transactionService.History(startDate, endDate));
    }
}

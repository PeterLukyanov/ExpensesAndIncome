using Services;
using Microsoft.AspNetCore.Mvc;
using Db;

namespace Controllers;

//This controller is for calculating the total balance of the account

[ApiController]
[Route("[controller]")]
public class TotalSumController : ControllerBase
{
    private readonly ExpensesAndIncomesDb db;
    private readonly TotalSummService totalSummService;
    public TotalSumController(ExpensesAndIncomesDb _db, TotalSummService _totalSummService)
    {
        db = _db;
        totalSummService = _totalSummService;
    }
    [HttpGet("Total Balance")]
    public async Task<ActionResult<double>> GetTotalBalance()
    {
        var result = await totalSummService.TotalBalance();
        if (result.IsSuccess)
            return Ok(result.Value);
        else
            return NotFound(result.Error);
    }
}
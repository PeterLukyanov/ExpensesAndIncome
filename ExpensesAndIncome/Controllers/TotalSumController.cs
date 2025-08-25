using Services;
using Microsoft.AspNetCore.Mvc;
using Db;

namespace Controllers;

//This controller is for calculating the total balance of the account

[ApiController]
[Route("[controller]")]
public class TotalSumController : ControllerBase
{
    public ExpensesAndIncomesDb db;
    public TotalSummService totalSummService;
    public TotalSumController(ExpensesAndIncomesDb _db, TotalSummService _totalSummService)
    {
        db = _db;
        totalSummService = _totalSummService;
    }
    [HttpGet("Total Balance")]
    public async Task<ActionResult<double>> GetTotalBalance()
    {
        return await totalSummService.TotalBalance();
    }
}
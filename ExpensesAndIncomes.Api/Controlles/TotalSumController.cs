using Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Controllers;

//This controller is for calculating the total balance of the account

[ApiController]
[Route("[controller]")]
public class TotalSumController : ControllerBase
{
    private readonly TotalSumService totalSummService;
    public TotalSumController( TotalSumService _totalSummService)
    {
        totalSummService = _totalSummService;
    }

    //[Authorize(Roles = "SuperUser, User")]
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
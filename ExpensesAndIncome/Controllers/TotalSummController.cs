using Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class TotalSummController : ControllerBase
{
    private readonly TotalSummService totalSummService;
    public ListTypesOfExpenses listTypesOfExpenses;
    public ListTypesOfIncomes listTypesOfIncomes;
    public TotalSummController(ListTypesOfIncomes _listTypesOfIncomes, ListTypesOfExpenses _listTypesOfExpenses, TotalSummService _totalSummService)
    {
        totalSummService = _totalSummService;
        listTypesOfExpenses = _listTypesOfExpenses;
        listTypesOfIncomes = _listTypesOfIncomes;
    }
    [HttpGet("Total balance")]
    public double GetBalance()
    {
        return totalSummService.TotalBalance();
    }
}

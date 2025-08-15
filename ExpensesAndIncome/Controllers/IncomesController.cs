using Models;
using Services;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class IncomesController : ControllerBase
{
    public IncomesManipulator incomesManipulator;
    public ListTypesOfIncomes listTypesOfIncomes;


    public IncomesController(IncomesManipulator _incomesManipulator, ListTypesOfIncomes _listTypesOfIncomes)
    {
        incomesManipulator = _incomesManipulator;
        listTypesOfIncomes = _listTypesOfIncomes;
    }

    [HttpGet("All Incomes")]
    public ActionResult<List<Income>> GetAll()
    {
        if (incomesManipulator.InfoOfIncomes() == null)
            return NotFound("There are no Incomes for now");
        return incomesManipulator.InfoOfIncomes();
    }
    [HttpPost]
    public IActionResult AddIncome([FromBody] Income income)
    {
        if (income.Amount <= 0)
            return BadRequest("Not valid number");

        var typeOfIncome = listTypesOfIncomes.listTypeOfIncomes.FirstOrDefault(c => c.NameOfType == income.StringTypeOfIncomes);
        if (typeOfIncome == null)
            return BadRequest("This category does not exist");

        incomesManipulator.AddNewIncome(income);
        return Ok(income);
    }
}
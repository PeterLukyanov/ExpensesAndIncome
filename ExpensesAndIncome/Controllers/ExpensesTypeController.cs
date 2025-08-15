using Models;
using Services;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class ExpensesTypeController : ControllerBase
{
    private readonly ExpensesTypeManipulator expensesTypeManipulator;
    public ListTypesOfExpenses listTypesOfExpenses;
    public ExpensesTypeController(ExpensesTypeManipulator _expensesTypeManipulator, ListTypesOfExpenses _listTypesOfExpenses)
    {
        expensesTypeManipulator = _expensesTypeManipulator;
        listTypesOfExpenses = _listTypesOfExpenses;
    }

    [HttpGet("Types of Expenses")]
    public ActionResult<List<ListOfExpenses>> GetAll()
    {
        if (expensesTypeManipulator.InfoTypes() == null)
            return NotFound("There are no types of Expenses for now");
        return expensesTypeManipulator.InfoTypes();
    }

    [HttpGet("bytype/{type}")]
    public ActionResult<ListOfExpenses> GetByType(string type)
    {
        var result = expensesTypeManipulator.GetInfoOfType(type);
        if (result == null)
            return BadRequest();

        return result;
    }
    [HttpGet("Total summ of Expenses")]
    public double GetTotalSummOfExpenses()
    {
        return expensesTypeManipulator.TotalSummOfExpenses();
    }

    [HttpPost]
    public IActionResult AddType([FromBody] ListOfExpenses listOfExpenses)
    {
        var typeOfExpense = listTypesOfExpenses.listTypeOfExpenses.FirstOrDefault(c => c.NameOfType.ToLower() == listOfExpenses.NameOfType.Trim().ToLower());
        if (typeOfExpense == null)
        {
            expensesTypeManipulator.AddType(listOfExpenses);
            return Ok(listOfExpenses);
        }
        else 
            return BadRequest($"Name {listOfExpenses.NameOfType} is already exists, try another name");
    }
}
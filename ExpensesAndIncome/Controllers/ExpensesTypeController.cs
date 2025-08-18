using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class ExpensesTypesController : ControllerBase
{
    private readonly ExpensesTypesManipulator expensesTypesManipulator;
    public ListTypesOfExpenses listTypesOfExpenses;
    public ExpensesTypesController(ExpensesTypesManipulator _expensesTypesManipulator, ListTypesOfExpenses _listTypesOfExpenses)
    {
        expensesTypesManipulator = _expensesTypesManipulator;
        listTypesOfExpenses = _listTypesOfExpenses;
    }

    [HttpGet("TypesOfExpenses")]
    public ActionResult<List<ListOfExpenses>> GetAll()
    {
        if (expensesTypesManipulator.InfoTypes() == null)
            return NotFound("There are no types of Expenses for now");
        return expensesTypesManipulator.InfoTypes();
    }

    [HttpGet("{type}")]
    public ActionResult<ListOfExpenses> GetByType(string type)
    {
        var result = expensesTypesManipulator.GetInfoOfType(type);
        if (result == null)
            return BadRequest();

        return result;
    }
    [HttpGet("TotalSummOfExpenses")]
    public double GetTotalSummOfExpenses()
    {
        return expensesTypesManipulator.TotalSummOfExpenses();
    }

    [HttpPost]
    public IActionResult AddType([FromBody] ListOfExpenses listOfExpenses)
    {
        var typeOfExpense = listTypesOfExpenses.listTypeOfExpenses.FirstOrDefault(c => c.NameOfType.ToLower() == listOfExpenses.NameOfType.Trim().ToLower());
        if (typeOfExpense == null)
        {
            expensesTypesManipulator.AddType(listOfExpenses);
            return Ok(listOfExpenses);
        }
        else
            return BadRequest($"Name {listOfExpenses.NameOfType} is already exists, try another name");
    }
    [HttpPut("{nameOfType}")]
    public IActionResult Update(string nameOfType, [FromBody] ListOfExpenses listOfExpenses)
    {
        var existingType = expensesTypesManipulator.GetInfoOfType(listOfExpenses.NameOfType);
        if (existingType is null)
            return NotFound();

        expensesTypesManipulator.Update(listOfExpenses, nameOfType);

        return NoContent();
    }
    [HttpDelete("{nameOfType}")]
    public IActionResult Delete(string nameOfType)
    {
        expensesTypesManipulator.Delete(nameOfType);

        if (listTypesOfExpenses.listTypeOfExpenses.FirstOrDefault(c => c.NameOfType == nameOfType) == null)
            return Ok(nameOfType);
        else
            return StatusCode(500, "Something goes wrong");
    }
}
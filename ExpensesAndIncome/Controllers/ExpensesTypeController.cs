using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Dtos;

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
        var result =  expensesTypesManipulator.InfoTypes();
        if (result == null)
            return NotFound("There are no types of Expenses for now");

        return result;
    }

    [HttpGet("{type}")]
    public ActionResult<ListOfExpenses> GetByType(string type)
    {
        var result = expensesTypesManipulator.GetInfoOfType(type);
        if (result == null)
            return BadRequest("Such type of Expenses does not exist");

        return result;
    }
    [HttpGet("TotalSummOfExpenses")]
    public double GetTotalSummOfExpenses()
    {
        return expensesTypesManipulator.TotalSummOfExpenses();
    }

    [HttpPost]
    public async Task <IActionResult> AddType([FromBody] TypeOfExpensesDto listOfExpenses)
    {
        var typeOfExpense = listTypesOfExpenses.ListTypeOfExpenses.FirstOrDefault(c => c.NameOfType.ToLower() == listOfExpenses.NameOfType.Trim().ToLower());
        if (typeOfExpense == null)
        {
            await expensesTypesManipulator.AddType(listOfExpenses);
            return Ok(listOfExpenses);
        }
        else
            return BadRequest($"Name {listOfExpenses.NameOfType} is already exists, try another name");
    }
    [HttpPut("{nameOfType}")]
    public async Task<IActionResult> Update( [FromBody] TypeOfExpensesDto listOfExpenses,string nameOfType)
    {
        var existingType = expensesTypesManipulator.GetInfoOfType(listOfExpenses.NameOfType);
        if (existingType is null)
            return NotFound("Such type of Expenses does not exist");

        await expensesTypesManipulator.Update(listOfExpenses, nameOfType);

        return Ok(nameOfType);
    }
    [HttpDelete("{nameOfType}")]
    public async Task<IActionResult> Delete(string nameOfType)
    {
        if (listTypesOfExpenses.ListTypeOfExpenses.FirstOrDefault(c => c.NameOfType == nameOfType)!=null)
        {
            await expensesTypesManipulator.Delete(nameOfType);
            return Ok(nameOfType);
        }
        else
            return BadRequest("Such type of Expenses does not exist");
    }
}
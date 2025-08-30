using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Dtos;
using Db;
using Microsoft.EntityFrameworkCore;

namespace Controllers;

//This controller processes requests: show all expense types,
// show all information about a specific type, display the total amount of expenses,
// add a new expense type, update an expense type, delete an expense type

[ApiController]
[Route("[controller]")]
public class ExpensesTypesController : ControllerBase
{
    private readonly ExpensesTypesManipulator expensesTypesManipulator;
    public ExpensesAndIncomesDb db;

    public ExpensesTypesController(ExpensesTypesManipulator _expensesTypesManipulator, ExpensesAndIncomesDb _db)
    {
        expensesTypesManipulator = _expensesTypesManipulator;
        db = _db;
    }
    //This query returns a list of all expense types.
    [HttpGet("TypesOfExpenses")]
    public async Task<ActionResult<List<string>>> GetAll()
    {
        var result = await expensesTypesManipulator.InfoTypes();
        if (result.Count == 0)
            return NotFound("There are no types of Expenses for now");

        return Ok(result);
    }
    //This query displays all information on the specified expense type: a list of all expenses for the selected type,
    //  the type name, the total expense amount for the type
    [HttpGet("{type}")]
    public async Task<ActionResult<ListOfExpenses>> GetByType(string type)
    {
        var result = await expensesTypesManipulator.GetInfoOfType(type);
        if (result == null)
            return BadRequest("Such type of Expenses does not exist");

        return result;
    }
    //This query returns the total amount of the expense.
    [HttpGet("TotalSummOfExpenses")]
    public async Task<double> GetTotalSummOfExpenses()
    {
        return await expensesTypesManipulator.TotalSummOfExpenses();
    }
    //This request adds a new expense type.
    [HttpPost]
    public async Task<IActionResult> AddType([FromBody] TypeOfExpensesDto listOfExpenses)
    {
        var typesOfExpense = await db.TypesOfExpenses.Select(t => t.Name).ToListAsync();
        var typeExist = typesOfExpense.FirstOrDefault(c => c.ToLower() == listOfExpenses.NameOfType.Trim().ToLower());
        if (typeExist == null)
        {
            await expensesTypesManipulator.AddType(listOfExpenses);
            return Ok(listOfExpenses);
        }
        else
            return BadRequest($"Name {listOfExpenses.NameOfType} is already exists, try another name");
    }
    //This request is to update the name of the expense type, while overwriting all expense objects marked with this expense type.
    [HttpPut("{nameOfType}")]
    public async Task<IActionResult> Update([FromBody] TypeOfExpensesDto listOfExpenses, string nameOfType)
    {
        var existingType = await expensesTypesManipulator.GetInfoOfType(listOfExpenses.NameOfType);
        if (existingType is null)
            return NotFound("Such type of Expenses does not exist");

        await expensesTypesManipulator.Update(listOfExpenses, nameOfType);

        return Ok(nameOfType);
    }
    //This request is to delete an expense type, which will delete all expense objects that are marked with this expense type.
    [HttpDelete("{nameOfType}")]
    public async Task<IActionResult> Delete(string nameOfType)
    {
        var listOfTypes = await db.TypesOfExpenses.Select(t => t.Name).ToListAsync();
        if (listOfTypes.FirstOrDefault(c => c == nameOfType) != null)
        {
            await expensesTypesManipulator.Delete(nameOfType);
            return Ok(nameOfType);
        }
        else
            return BadRequest("Such type of Expenses does not exist");
    }
}
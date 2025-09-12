using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Dtos;
using Db;
using Microsoft.AspNetCore.Authorization;

namespace Controllers;

//This controller processes requests: show all expense types,
// show all information about a specific type, display the total amount of expenses,
// add a new expense type, update an expense type, delete an expense type

[ApiController]
[Route("[controller]")]
public class ExpensesTypesController : ControllerBase
{
    private readonly ExpensesTypesManipulator expensesTypesManipulator;

    public ExpensesTypesController(ExpensesTypesManipulator _expensesTypesManipulator)
    {
        expensesTypesManipulator = _expensesTypesManipulator;
    }

    //This query returns a list of all expense types.
    [Authorize(Roles = "SuperUser, User")]
    [HttpGet("TypesOfExpenses")]
    public async Task<ActionResult<List<string>>> GetAll()
    {
        var result = await expensesTypesManipulator.InfoTypes();
        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    //This query displays all information on the specified expense type: a list of all expenses for the selected type,
    //  the type name, the total expense amount for the type
    [Authorize(Roles = "SuperUser, User")]
    [HttpGet("{type}")]
    public async Task<ActionResult<ListOfExpenses>> GetByType(string type)
    {
        var result = await expensesTypesManipulator.GetInfoOfType(type);
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok(result.Value);
    }

    //This query returns the total amount of the expense.
    [Authorize(Roles = "SuperUser, User")]
    [HttpGet("TotalSummOfExpenses")]
    public async Task<ActionResult<double>> GetTotalSummOfExpenses()
    {
        var result = await expensesTypesManipulator.TotalSumOfExpenses();
        if (result.IsSuccess)
            return Ok(result.Value);
        else
            return NotFound(result.Error);
    }

    //This request adds a new expense type.
    [Authorize(Roles = "SuperUser, User")]
    [HttpPost]
    public async Task<IActionResult> AddType([FromBody] TypeOfExpensesDto typeOfExpenses)
    {
        var result = await expensesTypesManipulator.AddType(typeOfExpenses);
        if (result.IsSuccess)
            return Ok(result.Value);
        else
            return BadRequest(result.Error);
    }

    //This request is to update the name of the expense type, while overwriting all expense objects marked with this expense type.
    [Authorize(Roles = "SuperUser, User")]
    [HttpPut("{nameOfType}")]
    public async Task<IActionResult> Update([FromBody] TypeOfExpensesDto typeOfExpenses, string nameOfType)
    {
        var result = await expensesTypesManipulator.Update(typeOfExpenses, nameOfType);
        if (result.IsFailure)
            return NotFound(result.Error);
        else
            return Ok(result.Value);
    }

    //This request is to delete an expense type, which will delete all expense objects that are marked with this expense type.
    [Authorize(Roles = "SuperUser, User")]
    [HttpDelete("{nameOfType}")]
    public async Task<IActionResult> Delete(string nameOfType)
    {
        var result = await expensesTypesManipulator.Delete(nameOfType);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
            return BadRequest(result.Error);
    }
}
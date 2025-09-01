using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Dtos;
using Db;
using Microsoft.EntityFrameworkCore;
using CSharpFunctionalExtensions;

namespace Controllers;

//This controller processes requests: listing all expenses, adding an expense, deleting an expense

[ApiController]
[Route("[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly ExpensesManipulator expensesManipulator;
    private readonly ExpensesAndIncomesDb db;

    public ExpensesController(ExpensesManipulator _expensesManipulator, ExpensesAndIncomesDb _db)
    {
        expensesManipulator = _expensesManipulator;
        db = _db;
    }

    //Request to display a list of all expense items
    [HttpGet("AllExpenses")]
    public async Task<ActionResult<List<Expense>>> GetAll()
    {
        var result = await expensesManipulator.InfoOfExpenses();
        if (result.IsSuccess)
            return Ok(result.Value);
        else
            return NotFound(result.Error);
    }

    //Request to add a new expense item
    [HttpPost]
    public async Task<IActionResult> AddExpense([FromBody] ExpenseDto dto)
    {
        var result = await expensesManipulator.AddNewExpense(dto);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    //Request to delete a specific expense by ID
    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        var result = await expensesManipulator.Delete(Id);
        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }
        return Ok(result.Value);
    }
}
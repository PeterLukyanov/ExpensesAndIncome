using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Controllers;

//This controller processes requests: listing all expenses, adding an expense, deleting an expense

[ApiController]
[Route("[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly ExpensesManipulator expensesManipulator;
    private readonly ILogger<ExpensesController> logger;

    public ExpensesController(ExpensesManipulator _expensesManipulator, ILogger<ExpensesController> _logger)
    {
        expensesManipulator = _expensesManipulator;
        logger = _logger;
    }

    //Request to display a list of all expense items
    [Authorize(Roles = "SuperUser, User")]
    [HttpGet("AllExpenses")]
    public async Task<ActionResult<List<Expense>>> GetAll()
    {
        logger.LogInformation("Calling a service to execute a request to display the entire list of expenses.");
        var result = await expensesManipulator.InfoOfExpenses();
        if (result.IsSuccess)
            return Ok(result.Value);
        else
        {
            logger.LogWarning(result.Error);
            return NotFound(result.Error);
        }
    }

    //Request to add a new expense item
    [Authorize(Roles = "SuperUser, User")]
    [HttpPost]
    public async Task<IActionResult> AddExpense([FromBody] ExpenseDto dto)
    {
        logger.LogInformation("Calling the service to perform a request to add a new expense");
        var result = await expensesManipulator.AddNewExpense(dto);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            logger.LogWarning(result.Error);
            return NotFound(result.Error);
        }
    }

    //Request to delete a specific expense by ID
    [Authorize(Roles = "SuperUser, User")]
    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        logger.LogInformation("Calling a service to perform a request to delete an expense");
        var result = await expensesManipulator.Delete(Id);
        if (result.IsFailure)
        {
            logger.LogWarning(result.Error);
            return NotFound(result.Error);
        }
        return Ok(result.Value);
    }
}